using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;
using static WildlifeMortalities.Data.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public abstract class HarvestActivity : Activity
{
    [Column($"{nameof(HarvestActivity)}_{nameof(HrbsNumber)}")]
    public string HrbsNumber { get; set; } = string.Empty;
    public ActivityQueueItem ActivityQueueItem { get; set; } = null!;

    public abstract string GetAreaName(Report report);
}

public class HarvestActivityConfig : IEntityTypeConfiguration<HarvestActivity>
{
    public void Configure(EntityTypeBuilder<HarvestActivity> builder)
    {
        builder.ToTable(TableNameConstants.Activities);
    }
}
