using System.ComponentModel.DataAnnotations;
using System.Reflection;
using WildlifeMortalities.App.Features.MortalityReports;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue) =>
        enumValue.GetEnumValueCustomAttribute<DisplayAttribute>()
            ?.GetName() ?? $"Error: enum {enumValue} is missing a displayname attribute";

    private static T? GetEnumValueCustomAttribute<T>(this Enum enumValue) where T : Attribute =>
        enumValue
            .GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttributes<T>()
            .FirstOrDefault();

    private static bool IsTrappableSpecies(this AllSpecies enumValue) =>
        enumValue.GetEnumValueCustomAttribute<IsTrappableAttribute>() != null;

    private static bool IsOutfitterHuntableSpecies(this AllSpecies enumValue) =>
        enumValue.GetEnumValueCustomAttribute<IsOutfitterGuidedHuntableAttribute>() != null;

    private static bool IsSpecialGuideHuntableSpecies(this AllSpecies enumValue) =>
        enumValue.GetEnumValueCustomAttribute<IsSpecialGuidedHuntableAttribute>() != null;

    private static bool ISIndividualHuntableSpecies(this AllSpecies enumValue) =>
        enumValue.GetEnumValueCustomAttribute<IsIndividualHuntableAttribute>() != null;

    public static bool IsSelectable(this AllSpecies enumValue, MortalityReportType type) => type switch
    {
        MortalityReportType.IndividualHunt => enumValue.ISIndividualHuntableSpecies(),
        MortalityReportType.OutfitterGuidedHunt => enumValue.IsOutfitterHuntableSpecies(),
        MortalityReportType.SpecialGuidedHunt => enumValue.IsSpecialGuideHuntableSpecies(),
        MortalityReportType.Trapped => enumValue.IsTrappableSpecies(),
        _ => false
    };
}
