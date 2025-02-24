using Accelerator.Shared.Infrastructure.Entities.Staging;
using Accelerator.Shared.Infrastructure.Infrastructure;
using commercetools.Sdk.ImportApi.Models.Categories;
using commercetools.Sdk.ImportApi.Models.StandalonePrices;

namespace Accelerator.Commercetools.Importer.GenericEtlPipeline;

public static class LoadingWorkerFactory
{
    public static Type CreateGenericLoadingWorker<T>(
        T stagingType) where T : class => stagingType switch
        {
            CommercetoolsCategoryImport => typeof(LoadingWorker<,,>).MakeGenericType(stagingType.GetType(), typeof(StagingImportContext), typeof(ICategoryImport)),
            CommercetoolsStandalonePriceImport => typeof(LoadingWorker<,,>).MakeGenericType(stagingType.GetType(), typeof(StagingImportContext), typeof(IStandalonePriceImport)),
            _ => throw new ArgumentOutOfRangeException()
        };
}