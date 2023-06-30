using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using WildlifeMortalities.Data.Extensions;

namespace WildlifeMortalities.Data.Entities;

public class Violation
{
    private Violation() { }

    public Violation(Activity activity, RuleType rule, SeverityType severity, string description)
    {
        Activity = activity ?? throw new ArgumentNullException(nameof(activity));

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException(
                $"'{nameof(description)}' cannot be null or whitespace.",
                nameof(description)
            );
        }

        Rule = rule;
        Severity = severity;
        Description = description;
    }

    public int Id { get; set; }
    public Activity Activity { get; init; } = null!;
    public RuleType Rule { get; init; }
    public SeverityType Severity { get; init; }
    public string Description { get; set; } = string.Empty;

    public enum RuleType
    {
        BagLimit = 10,
        HarvestPeriod = 20,
        Authorization = 30,
        LateReport = 40,
        LateBioSubmission = 50,
    }

    public enum SeverityType
    {
        InternalError = 10,
        PotentiallyIllegal = 20,
        Illegal = 30
    }

    #region Violations


    internal static Violation IllegalHarvestPeriod(HarvestActivity activity, Report report) =>
        new(
            activity,
            RuleType.HarvestPeriod,
            SeverityType.Illegal,
            $"{(activity is HuntedActivity ? "Area" : "Concession")} {activity.GetAreaName(report)} is closed to {(activity is HuntedActivity ? "hunting" : "trapping")} for {activity.Mortality.Species.GetDisplayName().ToLower()} of {activity.Mortality.Sex!.GetDisplayName().ToLower()} sex on {activity.Mortality.DateOfDeath:yyyy-MM-dd}."
        );

    internal static Violation BagLimitExceeded(
        HarvestActivity activity,
        Report report,
        BagEntry entry
    ) =>
        new(
            activity,
            RuleType.BagLimit,
            SeverityType.Illegal,
            $"Bag limit exceeded for {string.Join(" and ", entry.GetSpeciesDescriptions())} in {activity.GetAreaName(report)} for {entry.BagLimitEntry.GetSeason()} season."
        );

    #endregion
}
