using System.Globalization;
using Accelerator.Commercetools.Importer.Shared.Extension;
// using Accelerator.Shared.Infrastructure.Entities.Landing.Generated;
using Accelerator.Shared.Infrastructure.Entities.Staging;
using commercetools.Sdk.Api.Models.Channels;
using commercetools.Sdk.Api.Models.StandalonePrices;
using Mapster;
using IMoneyType = commercetools.Sdk.ImportApi.Models.Common.IMoneyType;
using TypedMoney = commercetools.Sdk.ImportApi.Models.Common.TypedMoney;

namespace Accelerator.Commercetools.Importer.Mapping;

public class CommercetoolsMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // config.NewConfig<Category_Group_Subgroup_Partterm_mappingGenerated, CommercetoolsCategoryImport>()
        //     .Map(i => i.Id, j => Guid.NewGuid())
        //     .Map(i => i.Hash, j => j.GetObjectHashCode());
        //
        // config.NewConfig<Price_USIC_PRICE20240325152337ABGenerated, CommercetoolsStandalonePriceImport>()
        //     .Map(i => i.Id,  j => Guid.NewGuid())
        //     .Map(i => i.Sku, j => string.Concat(j.Mfg, "-", j.Part))
        //     .Map(i => i.Value, j => j.Price.ToString(CultureInfo.InvariantCulture))
        //     .Map(i => i.Hash, j => j.GetObjectHashCode())
        //     .Map(i => i.Channel, j => $"prices-pw-{j.Regionid}");
        //     
        // config.NewConfig<Price_USIC_PRICE20240325152337BCGenerated, CommercetoolsStandalonePriceImport>()
        //     .Map(i => i.Id,  j => Guid.NewGuid())
        //     .Map(i => i.Sku, j => string.Concat(j.Mfg, "-", j.Part))
        //     .Map(i => i.Value, j => j.Price.ToString(CultureInfo.InvariantCulture))
        //     .Map(i => i.Hash, j => j.GetObjectHashCode())
        //     .Map(i => i.Channel, j => $"prices-pw-{j.Regionid}");
        //
        // config.NewConfig<CommercetoolsStandalonePriceImport, StandalonePrice>()
        //     .Map(i => i.Sku, j => j.Sku)
        //     .Map(i => i.Value, j => new TypedMoney
        //     {
        //         Type = IMoneyType.CentPrecision,
        //         FractionDigits = 2,
        //         CentAmount = long.Parse(j.Value),
        //         CurrencyCode = "CAD"
        //     })
        //     .Map(i => i.Channel, j => new ChannelReference
        //     {
        //         Id = j.Channel
        //     })
        //     .Map(i => i.CreatedAt, j => j.CreatedAt);
    }
}