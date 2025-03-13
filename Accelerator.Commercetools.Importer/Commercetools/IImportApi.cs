using commercetools.Sdk.ImportApi.Models.Categories;
using commercetools.Sdk.ImportApi.Models.Prices;
using commercetools.Sdk.ImportApi.Models.StandalonePrices;

namespace Accelerator.Commercetools.Importer.Commercetools;

public interface IImportApi
{
    Task BatchInsert<T>(IList<T> importEntities);
    Task BatchInsertCategories(IList<CategoryImport> categories, string container);
    Task BatchInsertStandalonePrice(IList<StandalonePriceImport> prices, string container);
}