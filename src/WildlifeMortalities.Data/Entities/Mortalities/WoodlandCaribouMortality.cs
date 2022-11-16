namespace WildlifeMortalities.Data.Entities.Mortalities;

public class WoodlandCaribouMortality : Mortality
{
    public WoodlandCaribouHerd Herd { get; set; }

    public enum WoodlandCaribouHerd
    {
        Uninitialized = 0,
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
        Pelly,
        RedStone,
        Redstone,
        SouthNahanni,
        SwanLake,
        Tatchun,
        Tay,
        WolfLake
    }
}
