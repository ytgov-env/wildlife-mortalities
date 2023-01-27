using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class OutfitterAssistantGuideLicence : Authorization
{
    public int BigGameHuntingLicenceId { get; set; }
    public BigGameHuntingLicence BigGameHuntingLicence { get; set; } = default!;

    public override AuthorizationResult GetResult(Report report) =>
        throw new NotImplementedException();
}

public class OutfitterAssistantGuideLicenceConfig
    : IEntityTypeConfiguration<OutfitterAssistantGuideLicence>
{
    public void Configure(EntityTypeBuilder<OutfitterAssistantGuideLicence> builder) =>
        builder
            .ToTable("Authorizations")
            .HasOne(p => p.BigGameHuntingLicence)
            .WithOne(h => h.OutfitterAssistantGuideLicence)
            .OnDelete(DeleteBehavior.NoAction);
}
