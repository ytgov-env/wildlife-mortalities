using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Seasons;
using static WildlifeMortalities.Data.Constants;
using System.ComponentModel.DataAnnotations.Schema;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.Rules.BagLimit;

public class HuntingBagLimitEntry : BagLimitEntry
{
    protected HuntingBagLimitEntry() { }

    public HuntingBagLimitEntry(
        IEnumerable<GameManagementArea> areas,
        Species species,
        HuntingSeason season,
        DateTimeOffset periodStart,
        DateTimeOffset periodEnd,
        int maxValuePerPerson,
        Sex? sex = null,
        int? maxValueForThreshold = null
    )
        : base(
            species,
            season,
            periodStart,
            periodEnd,
            maxValuePerPerson,
            sex,
            maxValueForThreshold
        )
    {
        Areas = areas.ToList();
        Season = season;
    }

    public List<GameManagementArea> Areas { get; init; } = null!;

    [Column(nameof(SeasonId))]
    public int SeasonId { get; init; }
    public HuntingSeason Season { get; init; } = null!;

    public override Season GetSeason()
    {
        return Season;
    }

    protected override bool IsWithinArea(HarvestActivity activity, Report report)
    {
        return Areas.Any(x => x.Id == ((HuntedActivity)activity).GameManagementArea.Id);
    }
}

public class HuntingBagLimitEntryConfig : IEntityTypeConfiguration<HuntingBagLimitEntry>
{
    public void Configure(EntityTypeBuilder<HuntingBagLimitEntry> builder)
    {
        builder.ToTable(TableNameConstants.BagLimitEntries);
        builder
            .HasOne(x => x.Season)
            .WithMany(x => x.BagLimitEntries)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
