using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class ElkMortality : Mortality
{
    public ElkHerd Herd { get; set; }
    public enum ElkHerd
    {
        Uninitialized = 0,
        Braeburn,
        Takhini
    }
}

public class ElkMortalityConfig : IEntityTypeConfiguration<ElkMortality>
{
    public void Configure(EntityTypeBuilder<ElkMortality> builder)
    {
        builder
            .ToTable("Mortalities")
            .Property(w => w.Herd).HasColumnName(nameof(ElkMortality.ElkHerd));;
    }
}
