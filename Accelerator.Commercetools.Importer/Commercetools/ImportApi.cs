using commercetools.Sdk.ImportApi.Client;
using commercetools.Sdk.ImportApi.Models.Importrequests;

namespace Accelerator.Commercetools.Importer.Commercetools;

public class ImportApi : IImportApi
{
    private ProjectApiRoot Client { get; set; }
    private const int MaximumBatchLenght = 120_000;

    public ImportApi(ProjectApiRoot client)
    {
        Client = client;
    }

    public async Task BatchInsert<T>(IList<T> importEntities, string container) =>
        await (importEntities switch
        {
            IList<ICategoryImportRequest> categories => BatchInsertCategories(categories, container),
            IList<IPriceImportRequest> prices => BatchInsertPrice(prices, container),
            _ => throw new ArgumentOutOfRangeException(nameof(importEntities), importEntities, null)
        });

    public async Task BatchInsertCategories(IList<ICategoryImportRequest> categories, string container)
    {
        foreach (var categoryImportRequest in categories.Chunk(MaximumBatchLenght))
        {
            var tasks = categoryImportRequest
                .Select(i =>
                    Client.Categories()
                        .ImportContainers()
                        .WithImportContainerKeyValue(container)
                        .Post(i)
                        .ExecuteAsync()
                );
            
            await Task.WhenAll(tasks);
        }
    }

    public async Task BatchInsertPrice(IList<IPriceImportRequest> prices, string container)
    {
        
        foreach (var priceImportRequest in prices.Chunk(MaximumBatchLenght))
        {
            var tasks = priceImportRequest.Select(i =>
                Client.Prices()
                    .ImportContainers()
                    .WithImportContainerKeyValue(container)
                    .Post(i)
                    .ExecuteAsync()
            );

            await Task.WhenAll(tasks);
        }
    }
}

public interface IImportApi
{
    Task BatchInsert<T>(IList<T> importEntities, string container);
    Task BatchInsertCategories(IList<ICategoryImportRequest> categories, string container);
    Task BatchInsertPrice(IList<IPriceImportRequest> prices, string container);
}
