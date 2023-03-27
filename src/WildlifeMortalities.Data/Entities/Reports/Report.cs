using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Entities.Reports;

public abstract class Report
{
    public int Id { get; set; }
    public string Discriminator { get; set; } = null!;
    public string HumanReadableId { get; set; } = string.Empty;
    public string Season { get; set; } = string.Empty;
    public DateTimeOffset DateSubmitted { get; set; }

    //public User CreatedBy { get; set; } = null!;
    //public User LastModifiedBy { get; set; } = null!;
    public ReportPdf? Pdf { get; set; }

    public IEnumerable<Mortality> GetMortalities() =>
        this switch
        {
            ISingleMortalityReport single => new[] { single.GetMortality() },
            IMultipleMortalitiesReport multiple => multiple.GetMortalities(),
            _ => throw new NotImplementedException()
        };

    public IEnumerable<Activity> GetActivities() =>
        this switch
        {
            ISingleMortalityReport single => new[] { single.GetActivity() },
            IMultipleMortalitiesReport multiple => multiple.GetActivities(),
            _ => throw new NotImplementedException()
        };

    public abstract string GetHumanReadableIdPrefix();

    public string GenerateHumanReadableId()
    {
        var rand = new Random();
        // Excludes O and 0 to avoid users mixing them up
        const string Chars = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
        var newHumanReadableIdSuffix = new string(
            Enumerable.Repeat(Chars, 4).Select(s => s[rand.Next(s.Length)]).ToArray()
        );
        HumanReadableId = $"{GetHumanReadableIdPrefix()}-{newHumanReadableIdSuffix}";

        return HumanReadableId;
    }

    public abstract bool HasHuntingActivity();
}
