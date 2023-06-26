using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Reports;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Rules.BagLimit;

public abstract class BagLimitEntry
{
    public const int InfiniteMaxValuePerPerson = int.MaxValue;

    public abstract Season GetSeason();

    protected abstract bool IsWithinArea(HarvestActivity activity, Report report);

    public int Id { get; set; }
    public Species Species { get; set; }
    public Sex? Sex { get; set; }
    public DateTimeOffset PeriodStart { get; set; }
    public DateTimeOffset PeriodEnd { get; set; }

    // Existing use case: Different species in the same collection of areas have a combined limit (ex: spruce and ruffed grouse)
    // Hypothetical use case: A person is allowed to kill 5 moose, but only 2 females
    // Hypothetical use case: A person is allowed to kill 5 moose across the yukon, but a maximum of 2 moose in any specific area
    public List<BagLimitEntry> SharedWithDifferentSpeciesAndOrSex { get; set; } = null!;
    public List<BagEntry> BagEntries { get; set; } = null!;
    public List<ActivityQueueItem> ActivityQueue { get; set; } = null!;
    public int MaxValuePerPerson { get; set; }
    public int? MaxValueForThreshold { get; set; }

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
        ActivityQueue ??= new();

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
