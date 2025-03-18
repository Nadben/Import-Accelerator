using commercetools.Sdk.ImportApi.Client;
using commercetools.Sdk.ImportApi.Models.Categories;
using commercetools.Sdk.ImportApi.Models.Importrequests;
using commercetools.Sdk.ImportApi.Models.StandalonePrices;
using commercetools.Sdk.ImportApi.Models.Common;
using commercetools.Sdk.ImportApi.Models.Inventories;

namespace Accelerator.Commercetools.Importer.Commercetools;

public class ImportApi : IImportApi
{
    private readonly ProjectApiRoot _client;
    private const int MaximumImportLength = 220_000;
    private const int MaximumBatchLength = 20;

    public ImportApi(ProjectApiRoot client)
    {
        _client = client;
    }

    public async Task BatchInsert<T>(IList<T> importEntities) where T : IImportResource
    {
        switch (importEntities)
        {
            case IList<IInventoryImport> inventories:
                await ProcessBatchInsert(inventories,
                    Constants.Container.Prefix.Inventory,
                    IImportResourceType.Inventory,
                    (container, batch) =>
                        _client.Inventories()
                            .ImportContainers()
                            .WithImportContainerKeyValue(container)
                            .Post(new InventoryImportRequest { Resources = batch })
                            .ExecuteAsync());
                break;
            
            case IList<ICategoryImport> categories:
                await ProcessBatchInsert(categories, Constants.Container.Prefix.Category, IImportResourceType.Category,
                    (container, batch) =>
                        _client.Categories()
                            .ImportContainers()
                            .WithImportContainerKeyValue(container)
                            .Post(new CategoryImportRequest { Resources = batch })
                            .ExecuteAsync());
                break;
            
            case IList<IStandalonePriceImport> prices:
                await ProcessBatchInsert(prices, Constants.Container.Prefix.Price, IImportResourceType.Price,
                    (container, batch) =>
                        _client.StandalonePrices()
                            .ImportContainers()
                            .WithImportContainerKeyValue(container)
                            .Post(new StandalonePriceImportRequest { Resources = batch })
                            .ExecuteAsync());
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(importEntities), importEntities, null);
        }
    }

    private async Task ProcessBatchInsert<T>(IList<T> importEntities, string containerPrefix, IImportResourceType resourceType, Func<string, IList<T>, Task> importAction) where T : IImportResource
    {
        int number = 0;
        foreach (var chunk in importEntities.Chunk(MaximumImportLength))
        {
            var containerName = $"{containerPrefix}-{number}";
            await _client.EnsureContainerIsCreated(containerName, resourceType);

            var tasks = chunk.Chunk(MaximumBatchLength)
                .Select(batch => importAction(containerName, batch))
                .ToList();

            await Task.WhenAll(tasks);
            number++;
        }
    }
}