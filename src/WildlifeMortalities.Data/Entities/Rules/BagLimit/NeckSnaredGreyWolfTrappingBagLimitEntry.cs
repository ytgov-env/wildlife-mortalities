using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Seasons;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Rules.BagLimit;

public class NeckSnaredGreyWolfTrappingBagLimitEntry : TrappingBagLimitEntry
{
    private NeckSnaredGreyWolfTrappingBagLimitEntry() { }

    public NeckSnaredGreyWolfTrappingBagLimitEntry(
        IEnumerable<RegisteredTrappingConcession> concessions,
        TrappingSeason season,
        DateTimeOffset periodStart,
        DateTimeOffset periodEnd,
        int maxValuePerPerson,
        Sex? sex = null,
        int? maxValueForThreshold = null,
        string? thresholdName = null
    )
        : base(
            concessions,
            Species.GreyWolf,
            season,
            periodStart,
            periodEnd,
            maxValuePerPerson,
            sex,
            maxValueForThreshold,
            thresholdName
        ) { }

    public override bool Matches(HarvestActivity activity, Report report)
    {
        var baseResult = base.Matches(activity, report);
        if (!baseResult)
            return false;

        if (activity.Mortality is not GreyWolfMortality)
            return false;

        if (activity is not TrappedActivity trappedActivity)
            return false;

        return trappedActivity.HarvestMethod == TrappedActivity.HarvestMethodType.NeckSnare;
    }
}

public class NeckSnaredGreyWolfTrappingBagLimitEntryConfig
    : IEntityTypeConfiguration<NeckSnaredGreyWolfTrappingBagLimitEntry>
{
    public void Configure(EntityTypeBuilder<NeckSnaredGreyWolfTrappingBagLimitEntry> builder)
    {
        builder.ToTable(TableNameConstants.BagLimitEntries);
    }
}
