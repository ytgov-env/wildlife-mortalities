using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Extensions;

namespace WildlifeMortalities.Data.Rules.Authorizations;

public class BigGameHuntingLicenceRulePipelineItem : AuthorizationRulePipelineItem
{
    public override Task<bool> Process(Report report, AuthorizationRulePipelineContext context)
    {
        foreach (var item in report.GetActivities())
        {
            if (!item.Mortality.Species.IsBigGameSpecies())
            {
                continue;
            }

            var authorization = report
                .GetPerson()
                .Authorizations.GetValidAuthorization<BigGameHuntingLicence>(item);
            if (authorization == null)
            {
                context.Violations.Add(
                    new Violation
                    {
                        Activity = item,
                        Severity = Violation.ViolationSeverity.Illegal,
                        Description = "",
                        Rule = Violation.RuleType.Authorization
                    }
                );
            }
            else
            {
                context.UsedAuthorizations.Add(authorization);

                if (
                    authorization.Type is BigGameHuntingLicence.LicenceType.CanadianResident
                    && report is not SpecialGuidedHuntReport or OutfitterGuidedHuntReport
                )
                {
                    context.Violations.Add(
                        new Violation
                        {
                            Activity = item,
                            Severity = Violation.ViolationSeverity.Illegal,
                            Description = "Hunted without a guide.",
                            Rule = Violation.RuleType.Authorization
                        }
                    );
                }
            }
        }

        return Task.FromResult(true);
    }
}
