using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Entities.Reports;

public abstract class Report
{
    public int Id { get; set; }
    public string HumanReadableId { get; set; } = string.Empty;
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
        var newHumanReadableIdSuffix = rand.Next(0, 1_000_000).ToString("d6");
        HumanReadableId = $"{GetHumanReadableIdPrefix()}-{newHumanReadableIdSuffix}";

        return HumanReadableId;

        //string GetSeason()
        //{
        //    var startingDate = GetMortalities().FirstOrDefault()?.DateOfDeath ?? DateSubmitted.Date;
        //    int startYear;
        //    int endYear;
        //    if (startingDate.Month <= 3)
        //    {
        //        startYear = startingDate.Year - 1;
        //        endYear = startingDate.Year;
        //    }
        //    else
        //    {
        //        startYear = startingDate.Year;
        //        endYear = startingDate.Year + 1;
        //    }

        //    return $"{startYear % 100:00}/{endYear % 100:00}";
        //}
    }

    public abstract bool HasHuntingActivity();
}
