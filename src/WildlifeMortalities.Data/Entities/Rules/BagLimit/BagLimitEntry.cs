using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Reports;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Rules.BagLimit;

public abstract class BagLimitEntry
{
    public const int InfiniteMaxValuePerPerson = int.MaxValue;

    protected BagLimitEntry() { }

    protected BagLimitEntry(
        Species species,
        Season season,
        DateTimeOffset periodStart,
        DateTimeOffset periodEnd,
        int maxValuePerPerson,
        Sex? sex = null,
        int? maxValueForThreshold = null
    )
    {
        Species = species;
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
        MaxValuePerPerson = maxValuePerPerson;
        Sex = sex;
        MaxValueForThreshold = maxValueForThreshold;

        if (!season.IsValidSubset(periodStart, periodEnd))
        {
            throw new ArgumentException(
                $"The specified {nameof(periodStart)} and {nameof(periodEnd)} are not within the {nameof(season)}."
            );
        }
        SharedWithDifferentSpeciesAndOrSex = new();
        BagEntries = new();
        ActivityQueue = new();
    }

    public abstract Season GetSeason();

    protected abstract bool IsWithinArea(HarvestActivity activity, Report report);

    public int Id { get; init; }
    public Species Species { get; init; }
    public Sex? Sex { get; init; }
    public DateTimeOffset PeriodStart { get; init; }
    public DateTimeOffset PeriodEnd { get; init; }

    // Existing use case: Different species in the same collection of areas have a combined limit (ex: spruce and ruffed grouse)
    // Hypothetical use case: A person is allowed to kill 5 moose, but only 2 females
    // Hypothetical use case: A person is allowed to kill 5 moose across the yukon, but a maximum of 2 moose in any specific area
    public List<BagLimitEntry> SharedWithDifferentSpeciesAndOrSex { get; init; } = null!;
    public List<BagEntry> BagEntries { get; init; } = null!;
    public List<ActivityQueueItem> ActivityQueue { get; init; } = null!;
    public int MaxValuePerPerson { get; set; }

    //Todo: create violation on threshold exceeded
    public int? MaxValueForThreshold { get; set; }
    public bool IsThreshold => MaxValueForThreshold.HasValue;

    public virtual bool Matches(HarvestActivity activity, Report report)
    {
        return IsWithinArea(activity, report)
            && Species == activity.Mortality.Species
            && GetSeason().Id == report.Season.Id
            && PeriodStart <= activity.Mortality.DateOfDeath
            && PeriodEnd >= activity.Mortality.DateOfDeath
            && (!Sex.HasValue || Sex == activity.Mortality.Sex);
    }

    public void AddToQueue(HarvestActivity activity)
    {
        ActivityQueue.Add(new ActivityQueueItem { Activity = activity, BagLimitEntry = this });
        ReorderQueue();
    }

    public void RemoveFromQueue(HarvestActivity activity)
    {
        ActivityQueue.Remove(ActivityQueue.First(x => x.Activity.Id == activity.Id));
        ReorderQueue();
    }

    private void ReorderQueue()
    {
        var index = 1;
        foreach (
            var item in ActivityQueue
                .OrderBy(x => x.Activity.Mortality.DateOfDeath)
                .ThenBy(x => x.Activity.CreatedTimestamp)
        )
        {
            item.Position = index++;
        }
    }
}

public class BagLimitEntryConfig : IEntityTypeConfiguration<BagLimitEntry>
{
    public void Configure(EntityTypeBuilder<BagLimitEntry> builder)
    {
        builder.ToTable(TableNameConstants.BagLimitEntries);
        builder
            .HasIndex(
                x =>
                    new
                    {
                        x.Species,
                        x.Sex,
                        x.PeriodStart,
                        x.PeriodEnd
                    }
            )
            .IsUnique();
    }
}

public class ActivityQueueItem
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public HarvestActivity Activity { get; set; } = null!;
    public int BagLimitEntryId { get; set; }
    public BagLimitEntry BagLimitEntry { get; set; } = null!;
    public int Position { get; set; }
}

public class ActivityQueueItemConfig : IEntityTypeConfiguration<ActivityQueueItem>
{
    public void Configure(EntityTypeBuilder<ActivityQueueItem> builder)
    {
        builder.ToTable(TableNameConstants.ActivityQueueItems);
    }
}
