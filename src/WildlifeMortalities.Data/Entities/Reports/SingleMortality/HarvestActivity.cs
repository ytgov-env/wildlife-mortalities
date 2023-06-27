using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public abstract class HarvestActivity : Activity
{
    public ActivityQueueItem ActivityQueueItem { get; set; } = null!;
    public bool IsThreshold { get; set; }

    public abstract string GetAreaName(Report report);
}

public class HarvestActivityConfig : IEntityTypeConfiguration<HarvestActivity>
{
    public void Configure(EntityTypeBuilder<HarvestActivity> builder)
    {
        builder.ToTable(TableNameConstants.Activities);
    }
}
