using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Shared.Extensions;

namespace WildlifeMortalities.Shared.Services.Reports.QueryObjects;

public static class ReportDtoSelect
{
    public static IQueryable<ReportDto> MapReportToDto(this IQueryable<Report> reports)
    {
        return reports.Select(
            report =>
                new ReportDto
                {
                    Id = report.Id,
                    HumanReadableId = report.HumanReadableId,
                    Type = report.Discriminator,
                    OutfitterGuidedHuntResult = (
                        (OutfitterGuidedHuntReport)report
                    ).Result.GetDisplayName(),
                    SpecialGuidedHuntResult = (
                        (SpecialGuidedHuntReport)report
                    ).Result.GetDisplayName(),
                    EnvClientId =
                        report is IndividualHuntedMortalityReport
                            ? ((IndividualHuntedMortalityReport)report).Client.EnvClientId
                            : report is SpecialGuidedHuntReport
                                ? ((SpecialGuidedHuntReport)report).Client.EnvClientId
                                : report is OutfitterGuidedHuntReport
                                    ? ((OutfitterGuidedHuntReport)report).Client.EnvClientId
                                    : report is TrappedMortalitiesReport
                                        ? ((TrappedMortalitiesReport)report).Client.EnvClientId
                                        : null,
                    BadgeNumber =
                        report is HumanWildlifeConflictMortalityReport
                            ? ((HumanWildlifeConflictMortalityReport)report)
                                .ConservationOfficer
                                .BadgeNumber
                            : null,
                    Species =
                        report is IndividualHuntedMortalityReport
                            ? new Species[]
                            {
                                ((IndividualHuntedMortalityReport)report)
                                    .HuntedActivity
                                    .Mortality
                                    .Species
                            }
                            : report is SpecialGuidedHuntReport
                                ? ((SpecialGuidedHuntReport)report).HuntedActivities.Select(
                                    x => x.Mortality.Species
                                )
                                : report is OutfitterGuidedHuntReport
                                    ? ((OutfitterGuidedHuntReport)report).HuntedActivities.Select(
                                        x => x.Mortality.Species
                                    )
                                    : report is TrappedMortalitiesReport
                                        ? (
                                            (TrappedMortalitiesReport)report
                                        ).TrappedActivities.Select(x => x.Mortality.Species)
                                        : new Species[] { },
                    Season = report.Season,
                    DateSubmitted = report.DateSubmitted
                }
        );
    }
}
