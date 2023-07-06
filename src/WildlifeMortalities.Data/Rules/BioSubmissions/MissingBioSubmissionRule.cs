using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Extensions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;

namespace WildlifeMortalities.Data.Rules.BioSubmissions;

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
                //var type = bioSubmission.GetType();
                //var missingOrganicMaterial = type.GetProperties()
                //    .Select(
                //        x =>
                //            new
                //            {
                //                info = x,
                //                attribute = x.GetCustomAttributes(false)
                //                    .OfType<IsRequiredOrganicMaterialForBioSubmissionAttribute>()
                //                    .FirstOrDefault()
                //            }
                //    )
                //    .Where(x => x.attribute != null && !(bool)x.info.GetValue(bioSubmission)!)
                //    .Select(x => x.attribute!.DisplayName.ToLower())
                //    .ToArray();

                violations.Add(
                    new Violation(
                        activity,
                        Violation.RuleType.SomeRequiredSamplesNotSubmitted,
                        Violation.SeverityType.Illegal,
                        $"Some of the required samples for {activity.Mortality.Species.GetDisplayName().ToLower()} were not submitted."
                    //$"Some of the required samples for {activity.Mortality.Species.GetDisplayName().ToLower()} were not submitted. These are: {string.Join(", and ", missingOrganicMaterial)}"
                    )
                );
            }
        }

        return !isUsed
            ? RuleResult.NotApplicable
            : violations.Any()
                ? RuleResult.IsIllegal(violations)
                : RuleResult.IsLegal;
    }
}
