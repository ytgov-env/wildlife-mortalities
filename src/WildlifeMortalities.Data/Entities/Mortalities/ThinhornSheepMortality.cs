using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class ThinhornSheepMortality : Mortality<ThinhornSheepMortality>
{
    public ThinhornSheepBodyColour BodyColour { get; set; }
    public ThinhornSheepTailColour TailColour { get; set; }
}

public class ThinhornSheepMortalityConfig : IEntityTypeConfiguration<ThinhornSheepMortality>
{
    public void Configure(EntityTypeBuilder<ThinhornSheepMortality> builder)
    {
        builder.Property(m => m.BodyColour).HasConversion<string>();
        builder.Property(m => m.TailColour).HasConversion<string>();
    }
}

public enum ThinhornSheepBodyColour
{
    Uninitialized = 0,
    Dark = 1,
    Fannin = 2,
    White = 3
}

public enum ThinhornSheepTailColour
{
    Uninitialized = 0,
    Dark = 1,
    White = 2
}
