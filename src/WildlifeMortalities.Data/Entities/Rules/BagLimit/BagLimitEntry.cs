using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Entities.Rules.BagLimit;

public class BagLimitEntry
{
    public int Id { get; set; }
    public List<GameManagementArea> Areas { get; set; } = null!;
    public Species Species { get; set; }
    public Sex? Sex { get; set; }
    public Season Season { get; set; } = null!;
    public DateTimeOffset PeriodStart { get; set; }
    public DateTimeOffset PeriodEnd { get; set; }

    // Existing use case: Different species in the same collection of areas have a combined limit (ex: spruce and ruffed grouse)
    // Hypothetical use case: A person is allowed to kill 5 moose, but only 2 females
    // Hypothetical use case: A person is allowed to kill 5 moose across the yukon, but a maximum of 2 moose in any specific area
    public List<BagLimitEntry> SharedWithDifferentSpeciesAndOrSex { get; set; } = null!;
    public int MaxValuePerPerson { get; set; }

    public List<ActivityQueueItem> ActivityQueue { get; set; } = null!;
    public int? MaxValueForThreshold { get; set; }

    public virtual bool Matches(HuntedActivity activity, Season season)
    {
        return Areas.Any(x => x.Id == activity.GameManagementArea.Id)
            && Species == activity.Mortality.Species
            && Season.Id == season.Id
            && PeriodStart <= activity.Mortality.DateOfDeath
            && PeriodEnd >= activity.Mortality.DateOfDeath
            && (!Sex.HasValue || Sex == activity.Mortality.Sex);
    }

    public void AddToQueue(HuntedActivity activity)
    {
        ActivityQueue ??= new();

        ActivityQueue.Add(new ActivityQueueItem { Activity = activity, BagLimitEntry = this });
        ReorderQueue();
    }

    public void RemoveFromQueue(HuntedActivity activity)
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
        builder.OwnsMany(x => x.ActivityQueue).WithOwner(x => x.BagLimitEntry);
    }
}

public class ActivityQueueItem
{
    public int BagLimitEntryId { get; set; }
    public BagLimitEntry BagLimitEntry { get; set; } = null!;
    public int ActivityId { get; set; }
    public HuntedActivity Activity { get; set; } = null!;
    public int Position { get; set; }
}
