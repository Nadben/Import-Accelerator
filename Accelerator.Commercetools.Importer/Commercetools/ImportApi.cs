using commercetools.Sdk.ImportApi.Client;
using commercetools.Sdk.ImportApi.Models.Categories;
using commercetools.Sdk.ImportApi.Models.Common;
using commercetools.Sdk.ImportApi.Models.Importrequests;
using commercetools.Sdk.ImportApi.Models.Prices;

namespace Accelerator.Commercetools.Importer.Commercetools;

public class ImportApi : IImportApi
{
    private readonly ProjectApiRoot _client;
    private const int MaximumBatchLenght = 120_000;

    public ImportApi(ProjectApiRoot client)
    {
        _client = client;
    }

    public async Task BatchInsert<T>(IList<T> importEntities) =>
        await (importEntities switch
        {
            IList<CategoryImport> categories => BatchInsertCategories(categories, "Category"),
            IList<PriceImport> prices => BatchInsertPrice(prices, "Price"),
            _ => throw new ArgumentOutOfRangeException(nameof(importEntities), importEntities, null)
        });

    public async Task BatchInsertCategories(IList<CategoryImport> categories, string container)
    {
        var number = 0;
        foreach (IList<ICategoryImport> categoryImportRequest in categories.Chunk(MaximumBatchLenght))
        {
            var containerName = $"{container}-{number}";
            var task = _client
                    .EnsureContainerIsCreated(containerName, IImportResourceType.Category)
                    .Result
                    .Categories()
                    .ImportContainers()
                    .WithImportContainerKeyValue(containerName)
                    .Post(new CategoryImportRequest
                    {
                        Resources = categoryImportRequest
                    })
                    .ExecuteAsync();


            number++;
            await task;
        }
    }

    public async Task BatchInsertPrice(IList<PriceImport> prices, string container)
    {
        var number = 0;
        foreach (IList<IPriceImport> priceImportRequest in prices.Chunk(MaximumBatchLenght))
        {
            var containerName = $"{container}-{number}";
            var task = _client
                .EnsureContainerIsCreated(containerName, IImportResourceType.Price)
                .Result
                .Prices()
                .ImportContainers()
                .WithImportContainerKeyValue(container)
                .Post(new PriceImportRequest
                {
                    Resources = priceImportRequest
                })
                .ExecuteAsync();
            
            number++;
            await task;
        }
    }
}