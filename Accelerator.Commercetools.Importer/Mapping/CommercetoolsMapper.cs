using System.Globalization;
using System.Text.RegularExpressions;
using Accelerator.Commercetools.Importer.Shared.Extension;
using Accelerator.Shared.Infrastructure.Entities.Landing.Generated;
using Accelerator.Shared.Infrastructure.Entities.Staging;
using commercetools.Sdk.Api.Models.Channels;
using commercetools.Sdk.ImportApi.Models.Categories;
using commercetools.Sdk.ImportApi.Models.Common;
using commercetools.Sdk.ImportApi.Models.Customfields;
using commercetools.Sdk.ImportApi.Models.Inventories;
using commercetools.Sdk.ImportApi.Models.StandalonePrices;
using Mapster;

namespace Accelerator.Commercetools.Importer.Mapping;

public class LocalizedString : Dictionary<string, string>, ILocalizedString
{
    public LocalizedString() { }

    public LocalizedString(IDictionary<string, string> dictionary) : base(dictionary) { }
}

public partial class CommercetoolsMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Dictionary<string, string>, ILocalizedString>()
            .MapWith(dict => new LocalizedString(dict));

        config.NewConfig<Money, ITypedMoney>().MapWith(
            src => new Money
            {
                Type = src.Type,
                CentAmount = src.CentAmount,
                CurrencyCode = src.CurrencyCode,
                FractionDigits = src.FractionDigits
            }
        );

        config.NewConfig<ChannelKeyReference, IChannelKeyReference>().MapWith(
            src => new ChannelKeyReference
            {
                Key = src.Key,
                TypeId = src.TypeId
            }
        );
        
        config.NewConfig<Category_Group_Subgroup_Partterm_mappingGenerated, CommercetoolsCategoryImport>()
                .Map(i => i.Id, j => Guid.NewGuid())
                .Map(i => i.Hash, j => j.GetObjectHashCode())
                .Map(i => i.Slug, j => j.Parttermid)
                .Map(i => i.Description, j => j.Groupname)
                .Map(i => i.Name, j => j.Subgroupname);
            
        config.NewConfig<Price_USIC_PRICE20240325152337ABGenerated, CommercetoolsStandalonePriceImport>()
            .Map(i => i.Id,  j => Guid.NewGuid())
            .Map(i => i.Sku, j => string.Concat(j.Mfg, "-", j.Part))
            .Map(i => i.Value, j => j.Price.ToString(CultureInfo.InvariantCulture))
            .Map(i => i.Hash, j => j.GetObjectHashCode())
            .Map(i => i.Channel, j => $"prices-pw-{j.Regionid}"
                .ToLowerInvariant()
                .ReplaceLineEndings(string.Empty)
            );

        config.NewConfig<Inventory_USIC_INV20240325152337Generated, CommercetoolsInventoryImport>()
            .Map(i => i.Id, j => Guid.NewGuid())
            .Map(i => i.Sku, j => string.Concat(j.Mfg, "-", j.Part))
            .Map(i => i.QuantityOnStock, j => j.StockQty)
            .Map(i => i.SupplyChannel, j => $"inventory-pw-{j.LocationID}"
                .ToLowerInvariant()
                .ReplaceLineEndings(string.Empty));

        config.NewConfig<CommercetoolsStandalonePriceImport, StandalonePriceImport>()
            .Map(i => i.Key, j => KeyRegexRule().Replace(j.Sku, string.Empty))
            .Map(i => i.Sku, j => j.Sku)
            .Map(i => i.Value, j => new Money
            {
                FractionDigits = 2,
                CentAmount = long.Parse(j.Value.Replace(".", string.Empty)),
                CurrencyCode = "USD"
            })
            .Map(i => i.Channel, j => new ChannelKeyReference
            {
                Key = j.Channel
            })
            .Ignore(i => i.Custom);

        config.NewConfig<CommercetoolsInventoryImport, InventoryImport>()
            .Map(i => i.Sku, j => j.Sku)
            .Map(i => i.SupplyChannel, j => new ChannelReference
            {
                Id = j.SupplyChannel
            })
            .Map(i => i.QuantityOnStock, j => j.QuantityOnStock);
        
        config.NewConfig<CommercetoolsCategoryImport, CategoryImport>()
            .Map(i => i.Slug , j => new LocalizedString
            {
                {"en-US", j.Slug}
            })
            .Map(i => i.Description, j => new LocalizedString
            {
                {"en-US", j.Description}
            })
            .Map(i => i.Name , j => new LocalizedString
            {
                {"en-US", j.Name}
            });
    }

    [GeneratedRegex("[^A-Za-z0-9_-]")]
    private static partial Regex KeyRegexRule();
}