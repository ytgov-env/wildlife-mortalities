using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class CaribouMortality : Mortality
{
    public CaribouHerd Herd { get; set; }

    public enum CaribouHerd
    {
        Aishihik,
        Atlin,
        BonnetPlume,
        Boreal,
        Carcross,
        Chisana,
        ClearCreek,
        CoalRiver,
        EthelLake,
        Finlayson,
        Fortymile,
        HartRiver,
        Horseranch,
        Ibex,
        Klaza,
        Kluane,
        Laberge,
        LaBiche,
        LiardPlateau,
        LittleRancheria,
        MooseLake,
        Nelchina,
        Pelly,
        Porcupine,
        RedStone,
        Redstone,
        SouthNahanni,
        SwanLake,
        Tatchun,
        Tay,
        WolfLake
    }
}

public class CaribouMortalityConfig : IEntityTypeConfiguration<CaribouMortality>
{
    public void Configure(EntityTypeBuilder<CaribouMortality> builder)
    {
        builder
            .ToTable("Mortalities")
            .Property(w => w.Herd).HasColumnName(nameof(CaribouMortality.CaribouHerd));
    }
}
