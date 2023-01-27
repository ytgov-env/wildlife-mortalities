using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Entities.Reports;

public abstract class Report
{
    public int Id { get; set; }

    //Todo configure number generation
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

    public abstract string GetHumanReadableIdPrefix();

    public string GenerateHumanReadableId()
    {
        var season = GetMortalities().First().DateOfDeath ?? DateSubmitted.Date;

        var rand = new Random();
        var newHumanReadableIdSuffix = rand.Next(0, 10_000).ToString("d5");
        HumanReadableId = $"{GetHumanReadableIdPrefix()}-{season:MM/yy}-{newHumanReadableIdSuffix}";

        return HumanReadableId;
    }

    public abstract bool HasHuntingActivity();
}
