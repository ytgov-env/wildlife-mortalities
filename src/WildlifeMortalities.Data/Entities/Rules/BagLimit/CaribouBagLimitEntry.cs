using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using static WildlifeMortalities.Data.Entities.Mortalities.CaribouMortality;
using static WildlifeMortalities.Data.Constants;
using WildlifeMortalities.Data.Entities.Seasons;

namespace WildlifeMortalities.Data.Entities.Rules.BagLimit;

public class CaribouBagLimitEntry : HuntingBagLimitEntry
{
    private CaribouBagLimitEntry() { }

    public CaribouBagLimitEntry(
        IEnumerable<GameManagementArea> areas,
        IEnumerable<CaribouHerd> herds,
        HuntingSeason season,
        DateTimeOffset periodStart,
        DateTimeOffset periodEnd,
        int maxValuePerPerson,
        Sex? sex = null,
        int? maxValueForThreshold = null
    )
        : base(
            areas,
            Species.Caribou,
            season,
            periodStart,
            periodEnd,
            maxValuePerPerson,
            sex,
            maxValueForThreshold
        )
    {
        Herds = herds.ToList();
    }

    public List<CaribouHerd> Herds { get; init; } = null!;

    override public bool Matches(HarvestActivity activity, Report report)
    {
        var baseResult = base.Matches(activity, report);
        if (!baseResult)
            return false;

        if (activity.Mortality is not CaribouMortality caribouMortality)
            return false;

        return Herds.Contains(caribouMortality.Herd);
    }
}

public class CaribouBagLimitEntryConfig : IEntityTypeConfiguration<CaribouBagLimitEntry>
{
    public void Configure(EntityTypeBuilder<CaribouBagLimitEntry> builder)
    {
        builder.ToTable(TableNameConstants.BagLimitEntries);
        builder
            .Property(x => x.Herds)
            .HasConversion(
                v => string.Join(",", v.Select(e => e.ToString("D")).ToArray()),
                v =>
                    v.Split(new[] { ',' })
                        .Select(e => Enum.Parse(typeof(CaribouHerd), e))
                        .Cast<CaribouHerd>()
                        .ToList()
            )
            .HasMaxLength(200);
    }
}
