using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class OutfitterAssistantGuideLicence : Authorization
{
    public int BigGameHuntingLicenceId { get; set; }
    public BigGameHuntingLicence BigGameHuntingLicence { get; set; } = default!;
}

public class OutfitterAssistantGuideLicenceConfig : IEntityTypeConfiguration<OutfitterAssistantGuideLicence>
{
    public void Configure(EntityTypeBuilder<OutfitterAssistantGuideLicence> builder)
    {
        builder.HasOne(p => p.BigGameHuntingLicence)
            .WithOne(h => h.OutfitterAssistantGuideLicence)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
