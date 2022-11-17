using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.MortalityReports;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class HuntingSeal : Authorization
{
    public HuntedSpecies Species { get; set; }
    public int BigGameHuntingLicenceId { get; set; }
    public BigGameHuntingLicence BigGameHuntingLicence { get; set; } = null!;
    public HuntedMortalityReport? HuntedMortalityReport { get; set; }
    public override AuthorizationResult GetResult(MortalityReport report) => throw new NotImplementedException();
}

public class SealConfig : IEntityTypeConfiguration<HuntingSeal>
{
    public void Configure(EntityTypeBuilder<HuntingSeal> builder)
    {
        builder
            .ToTable("Authorizations")
            .HasOne(s => s.BigGameHuntingLicence)
            .WithMany(h => h.HuntingSeals)
            .HasForeignKey(s => s.BigGameHuntingLicenceId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}
