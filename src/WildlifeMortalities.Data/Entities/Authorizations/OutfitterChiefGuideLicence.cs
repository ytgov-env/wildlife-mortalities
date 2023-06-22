using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class OutfitterChiefGuideLicence : Authorization, IHasOutfitterAreas
{
    public List<OutfitterArea> OutfitterAreas { get; set; } = null!;

    public override AuthorizationResult GetResult(Report report) =>
        throw new NotImplementedException();

    public override string GetAuthorizationType() => "Outfitter chief guide licence";

    protected override void UpdateInternal(Authorization authorization)
    {
        if (authorization is not OutfitterChiefGuideLicence outfitterChiefGuideLicence)
        {
            throw new ArgumentException(
                $"Expected {nameof(OutfitterChiefGuideLicence)} but received {authorization.GetType().Name}"
            );
        }

        OutfitterAreas = outfitterChiefGuideLicence.OutfitterAreas;
    }
}

public class OutfitterChiefGuideLicenceConfig : IEntityTypeConfiguration<OutfitterChiefGuideLicence>
{
    public void Configure(EntityTypeBuilder<OutfitterChiefGuideLicence> builder) =>
        builder.ToTable(TableNameConstants.Authorizations);
}
