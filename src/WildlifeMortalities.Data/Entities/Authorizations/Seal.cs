using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.MortalityReports;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class Seal : Authorization
{
    public HuntedSpecies Species { get; set; }
    public int BigGameHuntingLicenceId { get; set; }
    public BigGameHuntingLicence BigGameHuntingLicence { get; set; } = null!;
    public HuntedMortalityReport? HuntedMortalityReport { get; set; }
}

public class SealConfig : IEntityTypeConfiguration<Seal>
{
    public void Configure(EntityTypeBuilder<Seal> builder)
    {
        builder.Property(s => s.Species).HasConversion<string>();
        builder.HasOne(s => s.BigGameHuntingLicence)
            .WithMany(h => h.Seals)
            .HasForeignKey(s => s.BigGameHuntingLicenceId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}
