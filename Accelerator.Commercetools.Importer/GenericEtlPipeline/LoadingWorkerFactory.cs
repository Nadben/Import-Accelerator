using Accelerator.Commercetools.Importer.Workflow;
using Accelerator.Shared.Infrastructure.Entities.Staging;
using Accelerator.Shared.Infrastructure.Infrastructure;
using commercetools.Sdk.ImportApi.Models.Categories;
using commercetools.Sdk.ImportApi.Models.Inventories;
using commercetools.Sdk.ImportApi.Models.StandalonePrices;

namespace Accelerator.Commercetools.Importer.GenericEtlPipeline;

public static class LoadingWorkerFactory
{
    public static Type CreateGenericLoadingWorker<T>(T stagingType) where T : GroupedType => stagingType switch
        {
            _ when ReferenceEquals(stagingType.Type, typeof(CommercetoolsCategoryImport)) =>
                typeof(LoadingWorker<,,>).MakeGenericType(typeof(CommercetoolsCategoryImport),
                    typeof(StagingImportContext),
                    typeof(CategoryImport)),
            
            _ when ReferenceEquals(stagingType.Type, typeof(CommercetoolsStandalonePriceImport)) =>
                typeof(LoadingWorker<,,>).MakeGenericType(typeof(CommercetoolsStandalonePriceImport), 
                    typeof(StagingImportContext),
                    typeof(StandalonePriceImport)),
            
            _ when ReferenceEquals(stagingType.Type, typeof(CommercetoolsInventoryImport)) => 
                typeof(LoadingWorker<,,>).MakeGenericType(typeof(CommercetoolsStandalonePriceImport), 
                    typeof(StagingImportContext),
                    typeof(InventoryImport)),
            
            _ => throw new ArgumentOutOfRangeException()
        };
}