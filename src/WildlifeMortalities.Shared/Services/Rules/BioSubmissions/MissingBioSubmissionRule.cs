using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Extensions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data;

namespace WildlifeMortalities.Shared.Services.Rules.BioSubmissions;

internal class MissingBioSubmissionRule : Rule
{
    public override async Task<RuleResult> Process(Report report, AppDbContext context)
    {
        if (
            report.GeneralizedReportType
            is not GeneralizedReportType.Hunted
                or GeneralizedReportType.Trapped
        )
        {
            return RuleResult.NotApplicable;
        }

        var violations = new List<Violation>();
        var isApplicable = false;
        foreach (
            var activity in report
                .GetActivities()
                .Where(x => x.Mortality is IHasBioSubmission)
                .OrderBy(x => x.Mortality.DateOfDeath)
        )
        {
            var mortality = (IHasBioSubmission)activity.Mortality;
            if (!mortality.SubTypeHasBioSubmission())
            {
                continue;
            }

            var bioSubmission =
                await context.BioSubmissions.GetBioSubmissionFromMortality(mortality)
                ?? (
                    context.ChangeTracker
                        .Entries<BioSubmission>()
                        .Select(x => x.Entity)
                        .GetBioSubmissionFromMortality(mortality)
                    ?? throw new Exception(
                        "Expected mortality to have a bio submission, but no bio submission found."
                    )
                );

            isApplicable = true;

            if (
                bioSubmission.RequiredOrganicMaterialsStatus
                is BioSubmissionRequiredOrganicMaterialsStatus.DidNotSubmit
            )
            {
                violations.Add(
                    new Violation(
                        activity,
                        Violation.RuleType.AllRequiredSamplesNotSubmitted,
                        Violation.SeverityType.Illegal,
                        $"All of the required samples for {activity.Mortality.Species.GetDisplayName().ToLower()} were not submitted."
                    )
                );
            }
            else if (
                bioSubmission.RequiredOrganicMaterialsStatus
                is BioSubmissionRequiredOrganicMaterialsStatus.PartiallySubmitted
            )
            {
                violations.Add(
                    new Violation(
                        activity,
                        Violation.RuleType.SomeRequiredSamplesNotSubmitted,
                        Violation.SeverityType.Illegal,
                        $"Some of the required samples for {activity.Mortality.Species.GetDisplayName().ToLower()} were not submitted."
                    )
                );
            }
        }

        return !isApplicable
            ? RuleResult.NotApplicable
            : violations.Any()
                ? RuleResult.IsIllegal(violations)
                : RuleResult.IsLegal;
    }
}
