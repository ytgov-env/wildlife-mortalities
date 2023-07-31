using System.Diagnostics;
using WildlifeMortalities.Data.Entities;
using static WildlifeMortalities.Data.Entities.Mortalities.CaribouMortality;

namespace WildlifeMortalities.Data.Extensions;

public static class GameManagementAreaExtensions
{
    private static readonly Dictionary<string, CaribouHerd> s_simpleHerdMapper =
        new()
        {
            { "1-01", CaribouHerd.Porcupine },
            { "1-02", CaribouHerd.Porcupine },
            { "1-03", CaribouHerd.Porcupine },
            { "1-04", CaribouHerd.Porcupine },
            { "1-05", CaribouHerd.Porcupine },
            { "1-06", CaribouHerd.Porcupine },
            { "1-07", CaribouHerd.Porcupine },
            { "1-08", CaribouHerd.Porcupine },
            { "1-09", CaribouHerd.Porcupine },
            { "1-10", CaribouHerd.Porcupine },
            { "1-11", CaribouHerd.Porcupine },
            { "1-12", CaribouHerd.Porcupine },
            { "1-13", CaribouHerd.Porcupine },
            { "1-14", CaribouHerd.Porcupine },
            { "1-15", CaribouHerd.Porcupine },
            { "1-16", CaribouHerd.Porcupine },
            { "1-17", CaribouHerd.Porcupine },
            { "1-18", CaribouHerd.Porcupine },
            { "1-19", CaribouHerd.Porcupine },
            { "1-20", CaribouHerd.Porcupine },
            { "1-21", CaribouHerd.Porcupine },
            { "1-22", CaribouHerd.Porcupine },
            { "1-23", CaribouHerd.Porcupine },
            { "1-24", CaribouHerd.Porcupine },
            { "1-25", CaribouHerd.Porcupine },
            { "1-26", CaribouHerd.Porcupine },
            { "1-27", CaribouHerd.Porcupine },
            { "1-28", CaribouHerd.Porcupine },
            { "1-29", CaribouHerd.Porcupine },
            { "1-30", CaribouHerd.Porcupine },
            { "1-31", CaribouHerd.Porcupine },
            { "1-32", CaribouHerd.Porcupine },
            { "1-33", CaribouHerd.Porcupine },
            { "1-34", CaribouHerd.Porcupine },
            { "1-35", CaribouHerd.Porcupine },
            { "1-36", CaribouHerd.Porcupine },
            { "1-37", CaribouHerd.Porcupine },
            { "1-38", CaribouHerd.Porcupine },
            { "1-39", CaribouHerd.Porcupine },
            { "1-40", CaribouHerd.Porcupine },
            { "1-41", CaribouHerd.Porcupine },
            { "1-42", CaribouHerd.Porcupine },
            { "1-43", CaribouHerd.Porcupine },
            { "1-44", CaribouHerd.Porcupine },
            { "1-45", CaribouHerd.Porcupine },
            { "1-46", CaribouHerd.Porcupine },
            { "1-47", CaribouHerd.Porcupine },
            { "1-48", CaribouHerd.Porcupine },
            { "1-49", CaribouHerd.Porcupine },
            { "1-50", CaribouHerd.Porcupine },
            { "1-51", CaribouHerd.Porcupine },
            { "1-52", CaribouHerd.Porcupine },
            { "1-53", CaribouHerd.Porcupine },
            { "1-54", CaribouHerd.Porcupine },
            { "1-55", CaribouHerd.Porcupine },
            { "1-56", CaribouHerd.Porcupine },
            { "1-57", CaribouHerd.Porcupine },
            { "1-58", CaribouHerd.Porcupine },
            { "1-59", CaribouHerd.Porcupine },
            { "1-60", CaribouHerd.Porcupine },
            { "1-61", CaribouHerd.Porcupine },
            { "1-62", CaribouHerd.Porcupine },
            { "1-63", CaribouHerd.Porcupine },
            { "1-64", CaribouHerd.Porcupine },
            { "1-65", CaribouHerd.Porcupine },
            { "1-66", CaribouHerd.Porcupine },
            { "1-67", CaribouHerd.Porcupine },
            { "1-68", CaribouHerd.Porcupine },
            { "1-69", CaribouHerd.Porcupine },
            { "1-70", CaribouHerd.Porcupine },
            { "1-71", CaribouHerd.Porcupine },
            { "1-72", CaribouHerd.Porcupine },
            { "2-01", CaribouHerd.Porcupine },
            { "2-02", CaribouHerd.Porcupine },
            { "2-03", CaribouHerd.Porcupine },
            { "2-04", CaribouHerd.Porcupine },
            { "2-05", CaribouHerd.Porcupine },
            { "2-06", CaribouHerd.Porcupine },
            { "2-07", CaribouHerd.Porcupine },
            { "2-08", CaribouHerd.Porcupine },
            { "2-09", CaribouHerd.Porcupine },
            { "2-10", CaribouHerd.Porcupine },
            { "2-11", CaribouHerd.Porcupine },
            { "2-12", CaribouHerd.Porcupine },
            { "2-13", CaribouHerd.Porcupine },
            { "2-14", CaribouHerd.Porcupine },
            { "2-15", CaribouHerd.Porcupine },
            { "2-17", CaribouHerd.Porcupine },
            { "2-18", CaribouHerd.Porcupine },
            { "2-19", CaribouHerd.Fortymile },
            { "2-20", CaribouHerd.Fortymile },
            { "2-21", CaribouHerd.Fortymile },
            { "2-22", CaribouHerd.Porcupine },
            { "2-24", CaribouHerd.Fortymile },
            { "2-25", CaribouHerd.HartRiver },
            { "2-26", CaribouHerd.Porcupine },
            { "2-29", CaribouHerd.HartRiver },
            { "2-30", CaribouHerd.Porcupine },
            { "2-31", CaribouHerd.Porcupine },
            { "2-32", CaribouHerd.Porcupine },
            { "2-33", CaribouHerd.Porcupine },
            { "2-34", CaribouHerd.Porcupine },
            { "2-35", CaribouHerd.Porcupine },
            { "2-36", CaribouHerd.Porcupine },
            { "2-37", CaribouHerd.Porcupine },
            { "2-38", CaribouHerd.Porcupine },
            { "2-40", CaribouHerd.HartRiver },
            { "2-41", CaribouHerd.HartRiver },
            { "2-42", CaribouHerd.Porcupine },
            { "2-43", CaribouHerd.Porcupine },
            { "2-44", CaribouHerd.Porcupine },
            { "2-45", CaribouHerd.BonnetPlume },
            { "2-46", CaribouHerd.HartRiver },
            { "2-47", CaribouHerd.HartRiver },
            { "2-48", CaribouHerd.HartRiver },
            { "2-49", CaribouHerd.HartRiver },
            { "2-50", CaribouHerd.HartRiver },
            { "2-51", CaribouHerd.HartRiver },
            { "2-52", CaribouHerd.ClearCreek },
            { "2-53", CaribouHerd.ClearCreek },
            { "2-54", CaribouHerd.ClearCreek },
            { "2-55", CaribouHerd.ClearCreek },
            { "2-56", CaribouHerd.ClearCreek },
            { "2-57", CaribouHerd.ClearCreek },
            { "2-58", CaribouHerd.ClearCreek },
            { "2-59", CaribouHerd.ClearCreek },
            { "2-60", CaribouHerd.HartRiver },
            { "2-61", CaribouHerd.HartRiver },
            { "2-62", CaribouHerd.Unknown },
            { "2-63", CaribouHerd.Unknown },
            { "2-64", CaribouHerd.BonnetPlume },
            { "2-65", CaribouHerd.BonnetPlume },
            { "2-66", CaribouHerd.Porcupine },
            { "2-67", CaribouHerd.Porcupine },
            { "2-68", CaribouHerd.Porcupine },
            { "2-69", CaribouHerd.Porcupine },
            { "2-70", CaribouHerd.BonnetPlume },
            { "2-71", CaribouHerd.BonnetPlume },
            { "2-72", CaribouHerd.BonnetPlume },
            { "2-73", CaribouHerd.BonnetPlume },
            { "2-74", CaribouHerd.BonnetPlume },
            { "2-75", CaribouHerd.BonnetPlume },
            { "2-76", CaribouHerd.BonnetPlume },
            { "2-77", CaribouHerd.BonnetPlume },
            { "2-78", CaribouHerd.BonnetPlume },
            { "2-79", CaribouHerd.BonnetPlume },
            { "2-80", CaribouHerd.BonnetPlume },
            { "2-81", CaribouHerd.BonnetPlume },
            { "2-82", CaribouHerd.BonnetPlume },
            { "2-83", CaribouHerd.BonnetPlume },
            { "2-84", CaribouHerd.BonnetPlume },
            { "2-85", CaribouHerd.BonnetPlume },
            { "2-86", CaribouHerd.BonnetPlume },
            { "2-87", CaribouHerd.BonnetPlume },
            { "2-88", CaribouHerd.BonnetPlume },
            { "2-89", CaribouHerd.BonnetPlume },
            { "2-90", CaribouHerd.BonnetPlume },
            { "2-91", CaribouHerd.Tay },
            { "2-92", CaribouHerd.RedStone },
            { "2-93", CaribouHerd.RedStone },
            { "3-01", CaribouHerd.Fortymile },
            { "3-02", CaribouHerd.Fortymile },
            { "3-03", CaribouHerd.Fortymile },
            { "3-04", CaribouHerd.Fortymile },
            { "3-05", CaribouHerd.Fortymile },
            { "3-06", CaribouHerd.Fortymile },
            { "3-07", CaribouHerd.Fortymile },
            { "3-08", CaribouHerd.Fortymile },
            { "3-09", CaribouHerd.Fortymile },
            { "3-10", CaribouHerd.Fortymile },
            { "3-11", CaribouHerd.Fortymile },
            { "3-12", CaribouHerd.Fortymile },
            { "3-13", CaribouHerd.Fortymile },
            { "3-14", CaribouHerd.Fortymile },
            { "3-15", CaribouHerd.Fortymile },
            { "3-16", CaribouHerd.Fortymile },
            { "3-17", CaribouHerd.Fortymile },
            { "3-18", CaribouHerd.EthelLake },
            { "3-19", CaribouHerd.Fortymile },
            { "3-20", CaribouHerd.Tatchun },
            { "4-01", CaribouHerd.EthelLake },
            { "4-02", CaribouHerd.EthelLake },
            { "4-03", CaribouHerd.EthelLake },
            { "4-04", CaribouHerd.EthelLake },
            { "4-05", CaribouHerd.Unknown },
            { "4-06", CaribouHerd.Unknown },
            { "4-07", CaribouHerd.Unknown },
            { "4-08", CaribouHerd.Tay },
            { "4-09", CaribouHerd.EthelLake },
            { "4-10", CaribouHerd.EthelLake },
            { "4-11", CaribouHerd.Tay },
            { "4-12", CaribouHerd.Tatchun },
            { "4-13", CaribouHerd.Tatchun },
            { "4-14", CaribouHerd.Tatchun },
            { "4-15", CaribouHerd.Tatchun },
            { "4-16", CaribouHerd.Tatchun },
            { "4-17", CaribouHerd.Tay },
            { "4-18", CaribouHerd.Tay },
            { "4-19", CaribouHerd.MooseLake },
            { "4-20", CaribouHerd.Tay },
            { "4-21", CaribouHerd.RedStone },
            { "4-22", CaribouHerd.RedStone },
            { "4-23", CaribouHerd.Tay },
            { "4-24", CaribouHerd.Tay },
            { "4-25", CaribouHerd.Tay },
            { "4-26", CaribouHerd.Tay },
            { "4-27", CaribouHerd.MooseLake },
            { "4-28", CaribouHerd.Tay },
            { "4-29", CaribouHerd.Tay },
            { "4-30", CaribouHerd.Tay },
            { "4-31", CaribouHerd.RedStone },
            { "4-32", CaribouHerd.RedStone },
            { "4-33", CaribouHerd.Tay },
            { "4-34", CaribouHerd.Tay },
            { "4-35", CaribouHerd.RedStone },
            { "4-36", CaribouHerd.Tay },
            { "4-37", CaribouHerd.Tay },
            { "4-38", CaribouHerd.Tay },
            { "4-39", CaribouHerd.Tay },
            { "4-40", CaribouHerd.Tay },
            { "4-41", CaribouHerd.Tay },
            { "4-42", CaribouHerd.Tatchun },
            { "4-43", CaribouHerd.Tay },
            { "4-44", CaribouHerd.Tay },
            { "4-45", CaribouHerd.Tay },
            { "4-46", CaribouHerd.Tay },
            { "4-47", CaribouHerd.Tay },
            { "4-48", CaribouHerd.Tay },
            { "4-49", CaribouHerd.Tay },
            { "4-50", CaribouHerd.Tay },
            { "4-51", CaribouHerd.Tay },
            { "4-52", CaribouHerd.Tatchun },
            { "5-01", CaribouHerd.Fortymile },
            { "5-02", CaribouHerd.Fortymile },
            { "5-03", CaribouHerd.Klaza },
            { "5-07", CaribouHerd.Chisana },
            { "5-08", CaribouHerd.Chisana },
            { "5-09", CaribouHerd.Klaza },
            { "5-10", CaribouHerd.Klaza },
            { "5-11", CaribouHerd.Klaza },
            { "5-12", CaribouHerd.Klaza },
            { "5-13", CaribouHerd.Klaza },
            { "5-14", CaribouHerd.Aishihik },
            { "5-15", CaribouHerd.Aishihik },
            { "5-16", CaribouHerd.Chisana },
            { "5-17", CaribouHerd.Chisana },
            { "5-18", CaribouHerd.Kluane },
            { "5-19", CaribouHerd.Aishihik },
            { "5-20", CaribouHerd.Kluane },
            { "5-21", CaribouHerd.Kluane },
            { "5-22", CaribouHerd.Klaza },
            { "5-23", CaribouHerd.Klaza },
            { "5-24", CaribouHerd.Klaza },
            { "5-25", CaribouHerd.Klaza },
            { "5-26", CaribouHerd.Klaza },
            { "5-27", CaribouHerd.Aishihik },
            { "5-28", CaribouHerd.Kluane },
            { "5-29", CaribouHerd.Aishihik },
            { "5-30", CaribouHerd.Aishihik },
            { "5-31", CaribouHerd.Aishihik },
            { "5-32", CaribouHerd.Aishihik },
            { "5-33", CaribouHerd.Aishihik },
            { "5-34", CaribouHerd.Aishihik },
            { "5-35", CaribouHerd.Aishihik },
            { "5-36", CaribouHerd.Aishihik },
            { "5-37", CaribouHerd.Aishihik },
            { "5-38", CaribouHerd.Aishihik },
            { "5-39", CaribouHerd.Aishihik },
            { "5-40", CaribouHerd.Aishihik },
            { "5-41", CaribouHerd.Aishihik },
            { "5-42", CaribouHerd.Aishihik },
            { "5-43", CaribouHerd.Aishihik },
            { "5-44", CaribouHerd.Aishihik },
            { "5-45", CaribouHerd.Aishihik },
            { "5-46", CaribouHerd.Aishihik },
            { "5-47", CaribouHerd.Aishihik },
            { "5-48", CaribouHerd.Aishihik },
            { "5-49", CaribouHerd.Unknown },
            { "5-50", CaribouHerd.Unknown },
            { "5-51", CaribouHerd.Chisana },
            { "6-01", CaribouHerd.Chisana },
            { "6-02", CaribouHerd.Chisana },
            { "6-03", CaribouHerd.Chisana },
            { "6-04", CaribouHerd.Chisana },
            { "6-05", CaribouHerd.Chisana },
            { "6-06", CaribouHerd.Chisana },
            { "6-07", CaribouHerd.Chisana },
            { "6-08", CaribouHerd.Kluane },
            { "6-09", CaribouHerd.Unknown },
            { "6-10", CaribouHerd.Unknown },
            { "6-11", CaribouHerd.Unknown },
            { "6-12", CaribouHerd.Unknown },
            { "6-13", CaribouHerd.Unknown },
            { "7-01", CaribouHerd.Unknown },
            { "7-02", CaribouHerd.Unknown },
            { "7-03", CaribouHerd.Unknown },
            { "7-04", CaribouHerd.Unknown },
            { "7-05", CaribouHerd.Unknown },
            { "7-06", CaribouHerd.Unknown },
            { "7-07", CaribouHerd.Unknown },
            { "7-08", CaribouHerd.Unknown },
            { "7-09", CaribouHerd.Unknown },
            { "7-10", CaribouHerd.Unknown },
            { "7-11", CaribouHerd.Unknown },
            { "7-12", CaribouHerd.Unknown },
            { "7-13", CaribouHerd.Ibex },
            { "7-14", CaribouHerd.Ibex },
            { "7-15", CaribouHerd.Ibex },
            { "7-16", CaribouHerd.Ibex },
            { "7-17", CaribouHerd.Ibex },
            { "7-18", CaribouHerd.Ibex },
            { "7-19", CaribouHerd.Ibex },
            { "7-20", CaribouHerd.Ibex },
            { "7-21", CaribouHerd.Ibex },
            { "7-22", CaribouHerd.Ibex },
            { "7-23", CaribouHerd.Ibex },
            { "7-24", CaribouHerd.Ibex },
            { "7-25", CaribouHerd.Ibex },
            { "7-26", CaribouHerd.Ibex },
            { "7-27", CaribouHerd.Ibex },
            { "7-28", CaribouHerd.Ibex },
            { "7-29", CaribouHerd.Ibex },
            { "7-30", CaribouHerd.Ibex },
            { "7-31", CaribouHerd.Ibex },
            { "7-32", CaribouHerd.Ibex },
            { "7-33", CaribouHerd.Ibex },
            { "7-34", CaribouHerd.Ibex },
            { "7-35", CaribouHerd.Ibex },
            { "7-36", CaribouHerd.Carcross },
            { "8-01", CaribouHerd.Pelly },
            { "8-02", CaribouHerd.Pelly },
            { "8-03", CaribouHerd.Pelly },
            { "8-04", CaribouHerd.Pelly },
            { "8-05", CaribouHerd.Pelly },
            { "8-06", CaribouHerd.Pelly },
            { "8-07", CaribouHerd.Pelly },
            { "8-08", CaribouHerd.Pelly },
            { "8-09", CaribouHerd.Pelly },
            { "8-10", CaribouHerd.Pelly },
            { "8-11", CaribouHerd.Pelly },
            { "8-12", CaribouHerd.Carcross },
            { "8-13", CaribouHerd.Carcross },
            { "8-14", CaribouHerd.Carcross },
            { "8-15", CaribouHerd.Carcross },
            { "8-16", CaribouHerd.Carcross },
            { "8-17", CaribouHerd.Carcross },
            { "8-18", CaribouHerd.Pelly },
            { "8-19", CaribouHerd.Pelly },
            { "8-20", CaribouHerd.Pelly },
            { "8-21", CaribouHerd.Pelly },
            { "8-22", CaribouHerd.Pelly },
            { "8-23", CaribouHerd.Pelly },
            { "8-24", CaribouHerd.Pelly },
            { "8-25", CaribouHerd.Pelly },
            { "8-26", CaribouHerd.Carcross },
            { "8-27", CaribouHerd.Carcross },
            { "9-01", CaribouHerd.Carcross },
            { "9-02", CaribouHerd.Carcross },
            { "9-03", CaribouHerd.Carcross },
            { "9-04", CaribouHerd.Carcross },
            { "9-05", CaribouHerd.Carcross },
            { "9-06", CaribouHerd.Carcross },
            { "9-07", CaribouHerd.Carcross },
            { "9-08", CaribouHerd.Carcross },
            { "9-09", CaribouHerd.Atlin },
            { "9-10", CaribouHerd.Atlin },
            { "9-11", CaribouHerd.Atlin },
            { "10-01", CaribouHerd.Pelly },
            { "10-02", CaribouHerd.Pelly },
            { "10-03", CaribouHerd.Pelly },
            { "10-04", CaribouHerd.WolfLake },
            { "10-05", CaribouHerd.Finlayson },
            { "10-06", CaribouHerd.Finlayson },
            { "10-07", CaribouHerd.Finlayson },
            { "10-08", CaribouHerd.Finlayson },
            { "10-09", CaribouHerd.Finlayson },
            { "10-10", CaribouHerd.WolfLake },
            { "10-11", CaribouHerd.WolfLake },
            { "10-12", CaribouHerd.WolfLake },
            { "10-13", CaribouHerd.WolfLake },
            { "10-14", CaribouHerd.WolfLake },
            { "10-15", CaribouHerd.WolfLake },
            { "10-16", CaribouHerd.WolfLake },
            { "10-17", CaribouHerd.Finlayson },
            { "10-18", CaribouHerd.Finlayson },
            { "10-19", CaribouHerd.Finlayson },
            { "10-20", CaribouHerd.Unknown },
            { "10-21", CaribouHerd.WolfLake },
            { "10-22", CaribouHerd.Carcross },
            { "10-23", CaribouHerd.WolfLake },
            { "10-24", CaribouHerd.WolfLake },
            { "10-25", CaribouHerd.WolfLake },
            { "10-26", CaribouHerd.WolfLake },
            { "10-27", CaribouHerd.WolfLake },
            { "10-28", CaribouHerd.WolfLake },
            { "10-29", CaribouHerd.LittleRancheria },
            { "10-30", CaribouHerd.LittleRancheria },
            { "10-31", CaribouHerd.LittleRancheria },
            { "10-32", CaribouHerd.LittleRancheria },
            { "11-01", CaribouHerd.RedStone },
            { "11-02", CaribouHerd.Finlayson },
            { "11-03", CaribouHerd.Finlayson },
            { "11-04", CaribouHerd.Finlayson },
            { "11-05", CaribouHerd.Finlayson },
            { "11-06", CaribouHerd.Finlayson },
            { "11-07", CaribouHerd.Finlayson },
            { "11-08", CaribouHerd.Finlayson },
            { "11-09", CaribouHerd.Finlayson },
            { "11-10", CaribouHerd.Finlayson },
            { "11-11", CaribouHerd.Finlayson },
            { "11-12", CaribouHerd.Finlayson },
            { "11-13", CaribouHerd.Finlayson },
            { "11-14", CaribouHerd.Finlayson },
            { "11-15", CaribouHerd.Finlayson },
            { "11-16", CaribouHerd.Finlayson },
            { "11-17", CaribouHerd.Finlayson },
            { "11-18", CaribouHerd.SouthNahanni },
            { "11-19", CaribouHerd.CoalRiver },
            { "11-20", CaribouHerd.Finlayson },
            { "11-21", CaribouHerd.Finlayson },
            { "11-22", CaribouHerd.Finlayson },
            { "11-23", CaribouHerd.Finlayson },
            { "11-24", CaribouHerd.CoalRiver },
            { "11-25", CaribouHerd.CoalRiver },
            { "11-26", CaribouHerd.CoalRiver },
            { "11-27", CaribouHerd.CoalRiver },
            { "11-28", CaribouHerd.CoalRiver },
            { "11-29", CaribouHerd.LittleRancheria },
            { "11-30", CaribouHerd.CoalRiver },
            { "11-31", CaribouHerd.CoalRiver },
            { "11-32", CaribouHerd.CoalRiver },
            { "11-33", CaribouHerd.CoalRiver },
            { "11-34", CaribouHerd.CoalRiver },
            { "11-35", CaribouHerd.CoalRiver },
            { "11-36", CaribouHerd.CoalRiver },
            { "11-37", CaribouHerd.Unknown },
            { "11-38", CaribouHerd.CoalRiver },
            { "11-39", CaribouHerd.CoalRiver },
            { "11-40", CaribouHerd.Unknown },
            { "11-41", CaribouHerd.Unknown },
            { "11-42", CaribouHerd.LaBiche },
            { "11-43", CaribouHerd.Unknown },
            { "11-44", CaribouHerd.LaBiche },
            { "11-45", CaribouHerd.LaBiche },
            { "11-46", CaribouHerd.Unknown },
        };

    private static readonly Dictionary<
        string,
        IEnumerable<(CaribouHerd Herd, DateTimeOffset Start, DateTimeOffset End)>
    > s_complexHerdMapper =
        new()
        {
            {
                "2-16",
                new[]
                {
                    (
                        CaribouHerd.HartRiver,
                        new DateTimeOffset(2023, 8, 1, 0, 0, 0, TimeSpan.FromHours(-7)),
                        new DateTimeOffset(2023, 10, 31, 23, 59, 59, TimeSpan.FromHours(-7))
                    ),
                    (
                        CaribouHerd.Porcupine,
                        new DateTimeOffset(2023, 11, 1, 0, 0, 0, TimeSpan.FromHours(-7)),
                        new DateTimeOffset(2024, 1, 31, 23, 59, 59, TimeSpan.FromHours(-7))
                    )
                }
            },
            {
                "2-23",
                new[]
                {
                    (
                        CaribouHerd.HartRiver,
                        new DateTimeOffset(2023, 8, 1, 0, 0, 0, TimeSpan.FromHours(-7)),
                        new DateTimeOffset(2023, 10, 31, 23, 59, 59, TimeSpan.FromHours(-7))
                    ),
                    (
                        CaribouHerd.Porcupine,
                        new DateTimeOffset(2023, 11, 1, 0, 0, 0, TimeSpan.FromHours(-7)),
                        new DateTimeOffset(2024, 1, 31, 23, 59, 59, TimeSpan.FromHours(-7))
                    )
                }
            },
            {
                "2-27",
                new[]
                {
                    (
                        CaribouHerd.HartRiver,
                        new DateTimeOffset(2023, 8, 1, 0, 0, 0, TimeSpan.FromHours(-7)),
                        new DateTimeOffset(2023, 10, 31, 23, 59, 59, TimeSpan.FromHours(-7))
                    ),
                    (
                        CaribouHerd.Porcupine,
                        new DateTimeOffset(2023, 11, 1, 0, 0, 0, TimeSpan.FromHours(-7)),
                        new DateTimeOffset(2024, 1, 31, 23, 59, 59, TimeSpan.FromHours(-7))
                    )
                }
            },
            {
                "2-28",
                new[]
                {
                    (
                        CaribouHerd.HartRiver,
                        new DateTimeOffset(2023, 8, 1, 0, 0, 0, TimeSpan.FromHours(-7)),
                        new DateTimeOffset(2023, 10, 31, 23, 59, 59, TimeSpan.FromHours(-7))
                    ),
                    (
                        CaribouHerd.Porcupine,
                        new DateTimeOffset(2023, 11, 1, 0, 0, 0, TimeSpan.FromHours(-7)),
                        new DateTimeOffset(2024, 1, 31, 23, 59, 59, TimeSpan.FromHours(-7))
                    )
                }
            },
            {
                "2-39",
                new[]
                {
                    (
                        CaribouHerd.HartRiver,
                        new DateTimeOffset(2023, 8, 1, 0, 0, 0, TimeSpan.FromHours(-7)),
                        new DateTimeOffset(2023, 10, 31, 23, 59, 59, TimeSpan.FromHours(-7))
                    ),
                    (
                        CaribouHerd.Porcupine,
                        new DateTimeOffset(2023, 11, 1, 0, 0, 0, TimeSpan.FromHours(-7)),
                        new DateTimeOffset(2024, 1, 31, 23, 59, 59, TimeSpan.FromHours(-7))
                    )
                }
            },
            {
                "5-04",
                new[]
                {
                    (
                        CaribouHerd.Chisana,
                        new DateTimeOffset(2023, 4, 1, 0, 0, 0, TimeSpan.FromHours(-7)),
                        new DateTimeOffset(2023, 11, 30, 23, 59, 59, TimeSpan.FromHours(-7))
                    ),
                    (
                        CaribouHerd.Nelchina,
                        new DateTimeOffset(2023, 12, 1, 0, 0, 0, TimeSpan.FromHours(-7)),
                        new DateTimeOffset(2024, 3, 31, 23, 59, 59, TimeSpan.FromHours(-7))
                    )
                }
            },
            {
                "5-05",
                new[]
                {
                    (
                        CaribouHerd.Chisana,
                        new DateTimeOffset(2023, 4, 1, 0, 0, 0, TimeSpan.FromHours(-7)),
                        new DateTimeOffset(2023, 11, 30, 23, 59, 59, TimeSpan.FromHours(-7))
                    ),
                    (
                        CaribouHerd.Nelchina,
                        new DateTimeOffset(2023, 12, 1, 0, 0, 0, TimeSpan.FromHours(-7)),
                        new DateTimeOffset(2024, 3, 31, 23, 59, 59, TimeSpan.FromHours(-7))
                    )
                }
            },
            {
                "5-06",
                new[]
                {
                    (
                        CaribouHerd.Chisana,
                        new DateTimeOffset(2023, 4, 1, 0, 0, 0, TimeSpan.FromHours(-7)),
                        new DateTimeOffset(2023, 11, 30, 23, 59, 59, TimeSpan.FromHours(-7))
                    ),
                    (
                        CaribouHerd.Nelchina,
                        new DateTimeOffset(2023, 12, 1, 0, 0, 0, TimeSpan.FromHours(-7)),
                        new DateTimeOffset(2024, 3, 31, 23, 59, 59, TimeSpan.FromHours(-7))
                    )
                }
            },
        };

    public static bool IsPorcupineCaribou(
        this GameManagementArea area,
        DateTimeOffset dateOfDeath
    ) => area.GetLegalHerd(dateOfDeath) == CaribouHerd.Porcupine;

    public static CaribouHerd? GetLegalHerd(
        this GameManagementArea area,
        DateTimeOffset dateOfDeath
    )
    {
        var areaName = !string.IsNullOrWhiteSpace(area.Area)
            ? area.Area
            : $"{area.Zone}-{area.Subzone}";

        if (s_simpleHerdMapper.TryGetValue(areaName, out var herd))
        {
            return herd;
        }

        var date = dateOfDeath;

        if (s_complexHerdMapper.TryGetValue(areaName, out var herdTimeMapper))
        {
            var result = herdTimeMapper.Where(x => date >= x.Start && date <= x.End).ToArray();
            if (result.Length == 0)
            {
                return null;
            }
            else
            {
                return result[0].Herd;
            }
        }

        throw new UnreachableException();
    }

    public static string AreasToString(this IEnumerable<GameManagementArea> areas)
    {
        return string.Join(
            ", ",
            areas.OrderBy(x => x.Zone).ThenBy(x => x.Subzone).Select(x => x.Area).Distinct()
        );
    }
}
