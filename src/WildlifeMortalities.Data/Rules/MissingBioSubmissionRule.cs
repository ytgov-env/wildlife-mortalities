using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Extensions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;

namespace WildlifeMortalities.Data.Rules;

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
        var isUsed = false;
        foreach (
            var activity in report
                .GetActivities()
                .Where(x => x.Mortality is IHasBioSubmission)
                .OrderBy(x => x.Mortality.DateOfDeath)
        )
        {
            var bioSubmission =
                await context.BioSubmissions.GetBioSubmissionFromMortality(
                    (IHasBioSubmission)activity.Mortality
                )
                ?? throw new Exception(
                    "Expected mortality to have bio submission, but no bio submission found."
                );

            isUsed = true;

            if (
                bioSubmission.RequiredOrganicMaterialsStatus
                is BioSubmissionRequiredOrganicMaterialsStatus.DidNotSubmit
                    or BioSubmissionRequiredOrganicMaterialsStatus.PartiallySubmitted
            )
            {
                violations.Add(
                    new Violation(
                        activity,
                        Violation.RuleType.MissingBioSubmission,
                        Violation.SeverityType.Illegal,
                        $"Biological submission was not submitted for {activity.Mortality.Species.GetDisplayName().ToLower()}."
                    )
                );
            }
        }

        return !isUsed
            ? RuleResult.NotApplicable
            : (violations.Any() ? RuleResult.IsIllegal(violations) : RuleResult.IsLegal);
    }
}
