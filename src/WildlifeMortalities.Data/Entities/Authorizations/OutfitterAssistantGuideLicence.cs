using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class OutfitterAssistantGuideLicence : Authorization, IHasOutfitterAreas
{
    public List<OutfitterArea> OutfitterAreas { get; set; } = null!;

    public override string GetAuthorizationType() => "Outfitter assistant guide licence";

    protected override void UpdateInternal(Authorization authorization)
    {
        if (authorization is not OutfitterAssistantGuideLicence outfitterAssistantGuideLicence)
        {
            throw new ArgumentException(
                $"Expected {nameof(OutfitterAssistantGuideLicence)} but received {authorization.GetType().Name}"
            );
        }

        OutfitterAreas = outfitterAssistantGuideLicence.OutfitterAreas;
    }
}

public class OutfitterAssistantGuideLicenceConfig
    : IEntityTypeConfiguration<OutfitterAssistantGuideLicence>
{
    public void Configure(EntityTypeBuilder<OutfitterAssistantGuideLicence> builder) =>
        builder.ToTable(TableNameConstants.Authorizations);
}
