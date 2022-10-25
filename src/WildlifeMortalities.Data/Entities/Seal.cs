using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.MortalityReports;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities;

public class Seal
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public HuntedSpecies Species { get; set; }
    public int LicenceId { get; set; }
    public HuntingLicence Licence { get; set; } = null!;
    public HuntedMortalityReport? HuntedMortalityReport { get; set; }
}

public class SealConfig : IEntityTypeConfiguration<Seal>
{
    public void Configure(EntityTypeBuilder<Seal> builder)
    {
        builder.ToTable("Seals");
        builder.Property(s => s.Species).HasConversion<string>();
        // This shadow property is referenced during ETL to sync seal data from its source (POSSE)
        builder.Property<int?>("PosseId");
    }
}
