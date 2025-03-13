using commercetools.Sdk.ImportApi.Client;
using commercetools.Sdk.ImportApi.Models.Categories;
using commercetools.Sdk.ImportApi.Models.Common;
using commercetools.Sdk.ImportApi.Models.Importrequests;
using commercetools.Sdk.ImportApi.Models.Prices;
using commercetools.Sdk.ImportApi.Models.StandalonePrices;

namespace Accelerator.Commercetools.Importer.Commercetools;

public class ImportApi : IImportApi
{
    private readonly ProjectApiRoot _client;
    private const int MaximumImportLenght = 220_000;
    private const int MaximumBatchLenght = 20;

    public ImportApi(ProjectApiRoot client)
    {
        _client = client;
    }

    public async Task BatchInsert<T>(IList<T> importEntities) =>
        await (importEntities switch
        {
            IList<CategoryImport> categories => BatchInsertCategories(categories, "Category"),
            IList<StandalonePriceImport> prices => BatchInsertStandalonePrice(prices, "Price"),
            _ => throw new ArgumentOutOfRangeException(nameof(importEntities), importEntities, null)
        });

    public async Task BatchInsertCategories(IList<CategoryImport> categories, string container)
    {
        var number = 0;
        foreach (IList<ICategoryImport> categoryImportRequest in categories.Chunk(MaximumImportLenght))
        {
            var containerName = $"{container}-{number}";
            await _client.EnsureContainerIsCreated(containerName, IImportResourceType.Category);
            
            var tasks = categoryImportRequest.Chunk(MaximumBatchLenght)
                .Select(categoryImport => _client.Categories()
                    .ImportContainers()
                    .WithImportContainerKeyValue(containerName)
                    .Post(new CategoryImportRequest { Resources = categoryImport })
                    .ExecuteAsync())
                .Cast<Task>()
                .ToList();

            await Task.WhenAll(tasks);
            number++;
        }
    }

    public async Task BatchInsertStandalonePrice(IList<StandalonePriceImport> prices, string container)
    {
        var number = 0;
        foreach (IList<IStandalonePriceImport> priceImportRequest in prices.Chunk(MaximumImportLenght))
        {
            var containerName = $"{container}-{number}";
            await _client.EnsureContainerIsCreated(containerName, IImportResourceType.Price);

            var tasks = priceImportRequest.Chunk(MaximumBatchLenght)
                .Select(priceImport => _client.StandalonePrices()
                    .ImportContainers()
                    .WithImportContainerKeyValue(containerName)
                    .Post(new StandalonePriceImportRequest { Resources = priceImport })
                    .ExecuteAsync())
                .Cast<Task>()
                .ToList();

            await Task.WhenAll(tasks);
            number++;
        }
    }
}