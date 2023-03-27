using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
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
                    Season = report.Season,
                    DateSubmitted = report.DateSubmitted
                }
        );
    }

    public static IQueryable<ReportDto> MapReportToDto(
        this IQueryable<OutfitterGuidedHuntReport> reports
    )
    {
        return reports.Select(
            report =>
                new ReportDto
                {
                    Id = report.Id,
                    HumanReadableId = report.HumanReadableId,
                    EnvClientId = report.Client.EnvClientId,
                    Type = report.Discriminator,
                    OutfitterGuidedHuntResult = report.Result.GetDisplayName()
                }
        );
    }
}
