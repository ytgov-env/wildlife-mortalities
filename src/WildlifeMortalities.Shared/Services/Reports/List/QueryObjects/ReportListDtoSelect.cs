using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Shared.Extensions;

namespace WildlifeMortalities.Shared.Services.Reports.QueryObjects;

public static class ReportListDtoSelect
{
    public static IQueryable<ReportListDto> MapReportToDto(this IQueryable<Report> reports)
    {
        return reports.Select(
            report =>
                new ReportListDto
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
                            ? ((IndividualHuntedMortalityReport)report).Person.EnvPersonId
                            : report is SpecialGuidedHuntReport
                                ? ((SpecialGuidedHuntReport)report).Client.EnvPersonId
                                : report is OutfitterGuidedHuntReport
                                    ? ((OutfitterGuidedHuntReport)report).Client.EnvPersonId
                                    : report is TrappedMortalitiesReport
                                        ? ((TrappedMortalitiesReport)report).Client.EnvPersonId
                                        : null,
                    BadgeNumber =
                        report is HumanWildlifeConflictMortalityReport
                            ? ((HumanWildlifeConflictMortalityReport)report)
                                .ConservationOfficer
                                .BadgeNumber
                            : null,
                    Season = report.Season,
                    SpeciesCollection =
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
                    DateSubmitted = report.DateSubmitted
                }
        );
    }
}
