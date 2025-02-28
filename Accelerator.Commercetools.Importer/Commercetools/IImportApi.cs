using commercetools.Sdk.ImportApi.Models.Categories;
using commercetools.Sdk.ImportApi.Models.Prices;

namespace Accelerator.Commercetools.Importer.Commercetools;

public interface IImportApi
{
    Task BatchInsert<T>(IList<T> importEntities);
    Task BatchInsertCategories(IList<CategoryImport> categories, string container);
    Task BatchInsertPrice(IList<PriceImport> prices, string container);
}