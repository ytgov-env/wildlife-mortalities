using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class BarrenGroundCaribouMortality : Mortality
{
    public BarrenGroundCaribouHerd Herd { get; set; }
    public enum BarrenGroundCaribouHerd
    {
        Uninitialized = 0,
        Fortymile,
        Nelchina,
        Porcupine
    }
}

public class BarrenGroundCaribouMortalityConfig : IEntityTypeConfiguration<BarrenGroundCaribouMortality>
{
    public void Configure(EntityTypeBuilder<BarrenGroundCaribouMortality> builder)
    {
        builder
            .ToTable("Mortalities")
            .Property(w => w.Herd).HasColumnName(nameof(BarrenGroundCaribouMortality.BarrenGroundCaribouHerd));;
    }
}
