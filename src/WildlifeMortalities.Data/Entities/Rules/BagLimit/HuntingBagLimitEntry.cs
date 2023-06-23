using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Seasons;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Rules.BagLimit;

public class HuntingBagLimitEntry : BagLimitEntry
{
    public List<GameManagementArea> Areas { get; set; } = null!;

    public HuntingSeason Season { get; set; } = null!;

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
    }
}
