using commercetools.Sdk.ImportApi.Models.Importrequests;

namespace Accelerator.Commercetools.Importer.Commercetools;

public interface IImportApi
{
    Task BatchInsert<T>(IList<T> importEntities, string container);
    Task BatchInsertCategories(IList<ICategoryImportRequest> categories, string container);
    Task BatchInsertPrice(IList<IPriceImportRequest> prices, string container);
}