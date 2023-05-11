using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Users;

namespace WildlifeMortalities.Data.Entities.Reports;

[Flags]
public enum ReportStatus
{
    None = 0,
    BioMissing = 1,
    Violation = 2,
    Done = 4
}

public abstract class Report
{
    // Todo: Implement report status
    //public ReportStatus Status { get; set; }

    //public bool IsDone() => HasStatusFlag(ReportStatus.Done);

    //public bool HasStatusFlag(ReportStatus flag) => (Status & flag) == flag;

    //public void RemoveStatusFlag(ReportStatus flag) => Status &= ~flag;

    //public void AddStatusFlag(ReportStatus flag) => Status |= flag;

    public int Id { get; set; }
    public string Discriminator { get; set; } = null!;
    public string HumanReadableId { get; set; } = string.Empty;
    public Season Season { get; set; } = null!;
    public DateTimeOffset DateSubmitted { get; set; }

    public int CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;
    public DateTimeOffset DateCreated { get; set; }

    public int? LastModifiedById { get; set; }
    public User? LastModifiedBy { get; set; }
    public DateTimeOffset? DateModified { get; set; }

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

    public virtual DateTimeOffset GetRelevantDateForSeason()
    {
        return GetMortalities().FirstOrDefault()?.DateOfDeath ?? DateSubmitted;
    }

    public void GenerateHumanReadableId()
    {
        var rand = new Random();
        // Excludes O, 0, and I to avoid users mixing them up
        const string EligibleChars = "ABCDEFGHJKLMNPQRSTUVWXYZ123456789";
        var filteredWords = new[]
        {
            "FUCK",
            "SHIT",
            "PISS",
            "TWAT",
            "TITS",
            "CUNT",
            "SLUT",
            "SLAG",
            "PUSS",
            "DICK",
            "COCK",
            "GOOK",
            "KIKE",
            "SPIC",
            "COON",
            "PAKI",
            "JERK",
            "JISM",
            "DYKE",
            "ARSE",
            "HELL",
            "CRAP",
            "WANK",
            "WANG",
            "DAMN",
            "MONG",
            "DAGO",
            "SHAG",
            "TURD",
            "HOMO",
            "SCUM",
            "CLIT",
            "ANAL",
            "ANUS",
            "DUMB",
            "SUCK"
        };
        do
        {
            HumanReadableId = new string(
                Enumerable.Repeat(EligibleChars, 4).Select(s => s[rand.Next(s.Length)]).ToArray()
            );
        } while (!filteredWords.All(x => x != HumanReadableId));
    }

    public abstract bool HasHuntingActivity();
}

public class ReportConfig : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.HasIndex(r => r.HumanReadableId).IsUnique();
    }
}
