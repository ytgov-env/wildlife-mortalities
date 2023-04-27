using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class OutfitterChiefGuideLicence : Authorization, IHasOutfitterAreas
{
    public List<OutfitterArea> OutfitterAreas { get; set; } = null!;

    public override AuthorizationResult GetResult(Report report) =>
        throw new NotImplementedException();

    public override string GetAuthorizationType() => "Outfitter chief guide licence";
}

public class OutfitterChiefGuideLicenceConfig : IEntityTypeConfiguration<OutfitterChiefGuideLicence>
{
    public void Configure(EntityTypeBuilder<OutfitterChiefGuideLicence> builder) =>
        builder.ToTable("Authorizations");
}
