using commercetools.Sdk.ImportApi.Models.Common;

namespace Accelerator.Commercetools.Importer.Commercetools;

public interface IImportApi
{
    Task BatchInsert<T>(IList<T> importEntities) where T : IImportResource;
}