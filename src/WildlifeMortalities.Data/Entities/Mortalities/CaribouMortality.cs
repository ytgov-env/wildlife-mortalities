using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class CaribouMortality : Mortality, IHasBioSubmission
{
    public enum CaribouHerd
    {
        [Display(Name = "Unknown")]
        Unknown = -1,

        [Display(Name = "Aishihik")]
        Aishihik = 10,

        [Display(Name = "Atlin")]
        Atlin = 20,

        [Display(Name = "Bonnet plume")]
        BonnetPlume = 30,

        [Display(Name = "Boreal")]
        Boreal = 40,

        [Display(Name = "Carcross")]
        Carcross = 50,

        [Display(Name = "Chisana")]
        Chisana = 60,

        [Display(Name = "Clear creek")]
        ClearCreek = 70,

        [Display(Name = "Coal river")]
        CoalRiver = 80,

        [Display(Name = "Ethel lake")]
        EthelLake = 90,

        [Display(Name = "Finlayson")]
        Finlayson = 100,

        [Display(Name = "Fortymile")]
        Fortymile = 110,

        [Display(Name = "Hart river")]
        HartRiver = 120,

        [Display(Name = "Horseranch")]
        Horseranch = 130,

        [Display(Name = "Ibex")]
        Ibex = 140,

        [Display(Name = "Klaza")]
        Klaza = 150,

        [Display(Name = "Kluane")]
        Kluane = 160,

        [Display(Name = "Laberge")]
        Laberge = 170,

        [Display(Name = "La biche")]
        LaBiche = 180,

        [Display(Name = "Liard plateau")]
        LiardPlateau = 190,

        [Display(Name = "Little rancheria")]
        LittleRancheria = 200,

        [Display(Name = "Moose lake")]
        MooseLake = 210,

        [Display(Name = "Nelchina")]
        Nelchina = 220,

        [Display(Name = "Pelly")]
        Pelly = 230,

        [Display(Name = "Porcupine")]
        Porcupine = 240,

        [Display(Name = "Red stone")]
        RedStone = 250,

        [Display(Name = "South nahanni")]
        SouthNahanni = 260,

        [Display(Name = "Swan lake")]
        SwanLake = 270,

        [Display(Name = "Tatchun")]
        Tatchun = 280,

        [Display(Name = "Tay")]
        Tay = 290,

        [Display(Name = "Wolf lake")]
        WolfLake = 300
    }

    [Column($"{nameof(CaribouMortality)}_{nameof(LegalHerd)}")]
    public CaribouHerd LegalHerd { get; set; }

    [Column($"{nameof(CaribouMortality)}_{nameof(ActualHerd)}")]
    public CaribouHerd? ActualHerd { get; set; }

    public override Species Species => Species.Caribou;
    public CaribouBioSubmission? BioSubmission { get; set; }

    public BioSubmission? CreateDefaultBioSubmission() =>
        HerdHasBioSubmission() ? new CaribouBioSubmission(this) : null;

    bool IHasBioSubmission.SubTypeHasBioSubmission() => HerdHasBioSubmission();

    private bool HerdHasBioSubmission() =>
        LegalHerd is CaribouHerd.Fortymile or CaribouHerd.Nelchina;
}

public class CaribouMortalityConfig : IEntityTypeConfiguration<CaribouMortality>
{
    public void Configure(EntityTypeBuilder<CaribouMortality> builder) =>
        builder.ToTable(TableNameConstants.Mortalities);
}
