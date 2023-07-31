﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;
using WildlifeMortalities.Data.Entities.People;
using System.Text.Json.Serialization;

namespace WildlifeMortalities.Data.Entities.Reports;

[JsonDerivedType(typeof(IndividualHuntedMortalityReport), 1)]
public abstract class Report
{
    public int Id { get; set; }
    public string Discriminator { get; set; } = null!;
    public string HumanReadableId { get; set; } = string.Empty;
    public int SeasonId { get; set; }
    public Season Season { get; set; } = null!;
    public DateTimeOffset DateSubmitted { get; set; }

    public int CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;
    public DateTimeOffset DateCreated { get; set; }

    public int? LastModifiedById { get; set; }
    public User? LastModifiedBy { get; set; }
    public DateTimeOffset? DateModified { get; set; }

    public ReportPdf? Pdf { get; set; }

    [NotMapped]
    public abstract GeneralizedReportType GeneralizedReportType { get; }

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

    public abstract PersonWithAuthorizations GetPerson();

    public void PreserveImmutableValues(Report existingReport)
    {
        Discriminator = existingReport.Discriminator;
        HumanReadableId = existingReport.HumanReadableId;
        CreatedById = existingReport.CreatedById;
        DateCreated = existingReport.DateCreated;
    }

    public abstract void OverrideActivity(IDictionary<Activity, Activity> replacements);
}

public class ReportConfig : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.HasIndex(r => r.HumanReadableId).IsUnique();
    }
}

public enum GeneralizedReportType
{
    Hunted,
    Trapped,
    HumanWildlifeConflict,
    Collared,
    Research
}
