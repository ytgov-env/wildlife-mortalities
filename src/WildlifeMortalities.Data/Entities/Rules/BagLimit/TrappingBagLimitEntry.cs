using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Seasons;
using static WildlifeMortalities.Data.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace WildlifeMortalities.Data.Entities.Rules.BagLimit;

public class TrappingBagLimitEntry : BagLimitEntry
{
    protected TrappingBagLimitEntry() { }

    public TrappingBagLimitEntry(
        IEnumerable<RegisteredTrappingConcession> concessions,
        Species species,
        TrappingSeason season,
        DateTimeOffset periodStart,
        DateTimeOffset periodEnd,
        int maxValuePerPerson,
        Sex? sex = null,
        int? maxValueForThreshold = null,
        string? thresholdName = null
    )
        : base(
            species,
            season,
            periodStart,
            periodEnd,
            maxValuePerPerson,
            sex,
            maxValueForThreshold,
            thresholdName
        )
    {
        Concessions = concessions.ToList();
        Season = season;
    }

    public List<RegisteredTrappingConcession> Concessions { get; init; } = null!;

    [Column(nameof(SeasonId))]
    public int SeasonId { get; init; }
    public TrappingSeason Season { get; init; } = null!;

    public override Season GetSeason()
    {
        return Season;
    }

    protected override bool IsWithinArea(HarvestActivity activity, Report report)
    {
        return Concessions.Any(
            x => x.Id == ((TrappedMortalitiesReport)report).RegisteredTrappingConcession.Id
        );
    }
}

public class TrappingBagLimitEntryConfig : IEntityTypeConfiguration<TrappingBagLimitEntry>
{
    public void Configure(EntityTypeBuilder<TrappingBagLimitEntry> builder)
    {
        builder.ToTable(TableNameConstants.BagLimitEntries);
        builder
            .HasOne(x => x.Season)
            .WithMany(x => x.BagLimitEntries)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
