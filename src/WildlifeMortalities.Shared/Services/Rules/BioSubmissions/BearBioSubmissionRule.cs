using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Extensions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using static WildlifeMortalities.Data.Entities.Violation;

namespace WildlifeMortalities.Shared.Services.Rules.BioSubmissions;

internal class BearBioSubmissionRule : Rule
{
    public override IEnumerable<RuleType> ApplicableRuleTypes => new[] { RuleType.KilledBearCub };

    public override async Task<RuleResult> Process(Report report, AppDbContext context)
    {
        if (report.GeneralizedReportType is not GeneralizedReportType.Hunted)
        {
            return RuleResult.NotApplicable;
        }

        var violations = new List<Violation>();
        var isApplicable = false;
        foreach (
            var activity in report
                .GetActivities()
                .Where(x => x.Mortality is AmericanBlackBearMortality or GrizzlyBearMortality)
        )
        {
            var bioSubmission = await context.BioSubmissions.GetBioSubmissionFromMortality(
                (IHasBioSubmission)activity.Mortality
            );

            if (bioSubmission is null)
            {
                continue;
            }

            if (
                bioSubmission.AnalysisStatus is not BioSubmissionAnalysisStatus.Complete
                || bioSubmission.Age is null
            )
            {
                continue;
            }

            isApplicable = true;

            if (bioSubmission.Age.Years < 3)
            {
                violations.Add(
                    new Violation(
                        activity,
                        RuleType.KilledBearCub,
                        SeverityType.Illegal,
                        $"{activity.Mortality.Species.GetDisplayName()} is less than 3 years old."
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
