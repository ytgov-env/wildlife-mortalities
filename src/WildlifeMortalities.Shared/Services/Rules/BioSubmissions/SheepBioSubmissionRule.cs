using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Extensions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data;
using static WildlifeMortalities.Data.Entities.Violation;

namespace WildlifeMortalities.Shared.Services.Rules.BioSubmissions;

internal class SheepBioSubmissionRule : Rule
{
    public override IEnumerable<RuleType> ApplicableRuleTypes =>
        new[] { RuleType.SheepEyeSocketIncomplete, RuleType.SheepYoungerThan8AndNotFullCurl };

    public override async Task<RuleResult> Process(Report report, AppDbContext context)
    {
        if (report.GeneralizedReportType is not GeneralizedReportType.Hunted)
        {
            return RuleResult.NotApplicable;
        }

        var violations = new List<Violation>();
        var isApplicable = false;
        foreach (
            var activity in report.GetActivities().Where(x => x.Mortality is ThinhornSheepMortality)
        )
        {
            if (
                await context.BioSubmissions.GetBioSubmissionFromMortality(
                    (ThinhornSheepMortality)activity.Mortality
                )
                is not ThinhornSheepBioSubmission bioSubmission
            )
            {
                continue;
            }

            if (bioSubmission.AnalysisStatus is not BioSubmissionAnalysisStatus.Complete)
            {
                continue;
            }

            isApplicable = true;

            if (bioSubmission.Age!.Years < 8 && bioSubmission.IsFullCurl == false)
            {
                violations.Add(
                    new Violation(
                        activity,
                        RuleType.SheepYoungerThan8AndNotFullCurl,
                        SeverityType.Illegal,
                        "Sheep is under 8 years old and not full curl."
                    )
                );
            }

            if (bioSubmission.IsBothEyeSocketsComplete == false)
            {
                violations.Add(
                    new Violation(
                        activity,
                        RuleType.SheepEyeSocketIncomplete,
                        SeverityType.PotentiallyIllegal,
                        "Sheep has incomplete eye socket(s)."
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
