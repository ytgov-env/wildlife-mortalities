using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class CaribouMortality : Mortality, IHasBioSubmission
{
    public enum CaribouHerd
    {
        Aishihik = 10,
        Atlin = 20,
        BonnetPlume = 30,
        Boreal = 40,
        Carcross = 50,
        Chisana = 60,
        ClearCreek = 70,
        CoalRiver = 80,
        EthelLake = 90,
        Finlayson = 100,
        Fortymile = 110,
        HartRiver = 120,
        Horseranch = 130,
        Ibex = 140,
        Klaza = 150,
        Kluane = 160,
        Laberge = 170,
        LaBiche = 180,
        LiardPlateau = 190,
        LittleRancheria = 200,
        MooseLake = 210,
        Nelchina = 220,
        Pelly = 230,
        Porcupine = 240,
        RedStone = 250,
        Redstone = 260,
        SouthNahanni = 270,
        SwanLake = 280,
        Tatchun = 290,
        Tay = 300,
        WolfLake = 310
    }

    public CaribouHerd Herd { get; set; }

    public override Species Species => Species.Caribou;
    public CaribouBioSubmission? BioSubmission { get; set; }

    public BioSubmission CreateDefaultBioSubmission() => new CaribouBioSubmission(this);
}

public class CaribouMortalityConfig : IEntityTypeConfiguration<CaribouMortality>
{
    public void Configure(EntityTypeBuilder<CaribouMortality> builder) =>
        builder
            .ToTable("Mortalities")
            .Property(w => w.Herd)
            .HasColumnName(nameof(CaribouMortality.CaribouHerd));
}
