using WildlifeMortalities.App.Features.Reports;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Shared.Extensions;

namespace WildlifeMortalities.App.Extensions;

public static class SpeciesExtensions
{
    private static bool IsTrappableSpecies(this Species enumValue) =>
        enumValue.GetEnumValueCustomAttribute<IsTrappableAttribute>() != null;

    private static bool IsOutfitterHuntableSpecies(this Species enumValue) =>
        enumValue.GetEnumValueCustomAttribute<IsOutfitterGuidedHuntableAttribute>() != null;

    private static bool IsSpecialGuideHuntableSpecies(this Species enumValue) =>
        enumValue.GetEnumValueCustomAttribute<IsSpecialGuidedHuntableAttribute>() != null;

    private static bool IsIndividualHuntableSpecies(this Species enumValue) =>
        enumValue.GetEnumValueCustomAttribute<IsIndividualHuntableAttribute>() != null;

    public static bool IsSelectable(this Species enumValue, MortalityReportType type) =>
        type switch
        {
            MortalityReportType.IndividualHunt => enumValue.IsIndividualHuntableSpecies(),
            MortalityReportType.OutfitterGuidedHunt => enumValue.IsOutfitterHuntableSpecies(),
            MortalityReportType.SpecialGuidedHunt => enumValue.IsSpecialGuideHuntableSpecies(),
            MortalityReportType.Trapped => enumValue.IsTrappableSpecies(),
            _ => false
        };
}
