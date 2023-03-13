using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Authorizations;

namespace WildlifeMortalities.Data.Entities;

public class OutfitterArea
{
    public int Id { get; set; }
    public string Area { get; set; } = string.Empty;
    public List<BigGameHuntingLicence> BigGameHuntingLicences { get; set; } = null!;
    public List<SmallGameHuntingLicence> SmallGameHuntingLicences { get; set; } = null!;
    public List<OutfitterChiefGuideLicence> OutfitterChiefGuideLicences { get; set; } = null!;
    public List<OutfitterAssistantGuideLicence> OutfitterAssistantGuideLicences { get; set; } =
        null!;
}

public class OutfitterAreaConfig : IEntityTypeConfiguration<OutfitterArea>
{
    public void Configure(EntityTypeBuilder<OutfitterArea> builder) =>
        builder.HasIndex(a => a.Area).IsUnique();
}
