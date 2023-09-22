using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using static WildlifeMortalities.Data.Entities.Mortalities.CaribouMortality;
using static WildlifeMortalities.Data.Constants;
using WildlifeMortalities.Data.Entities.Seasons;
using System.Diagnostics;
using WildlifeMortalities.Data.Extensions;

namespace WildlifeMortalities.Data.Entities.Rules.BagLimit;

public class CaribouBagLimitEntry : HuntingBagLimitEntry
{
    private readonly bool _isPorcupineHerd;

    private CaribouBagLimitEntry() { }

    public CaribouBagLimitEntry(
        IEnumerable<GameManagementArea> areas,
        HuntingSeason season,
        DateTimeOffset periodStart,
        DateTimeOffset periodEnd,
        int maxValuePerPerson,
        Sex? sex = null,
        int? maxValueForThreshold = null,
        string? thresholdName = null
    )
        : base(
            areas,
            Species.Caribou,
            season,
            periodStart,
            periodEnd,
            maxValuePerPerson,
            sex,
            maxValueForThreshold,
            thresholdName
        )
    {
        bool? isPorcupineHerd = null;
        foreach (var area in areas)
        {
            var startIsPorcupineHerd = area.IsPorcupineCaribou(PeriodStart);
            var endIsPorcupineHerd = area.IsPorcupineCaribou(PeriodEnd);

            if (startIsPorcupineHerd != endIsPorcupineHerd)
            {
                throw new Exception();
            }

            if (isPorcupineHerd != null)
            {
                if (startIsPorcupineHerd != isPorcupineHerd)
                {
                    throw new Exception();
                }
            }
            else
            {
                isPorcupineHerd = startIsPorcupineHerd;
            }
        }
        if (isPorcupineHerd == null)
        {
            throw new UnreachableException();
        }

        _isPorcupineHerd = isPorcupineHerd.Value;
    }

    public override bool ShouldMutateBagValue(HarvestActivity activity)
    {
        if (activity is not HuntedActivity huntedActivity)
        {
            throw new UnreachableException();
        }

        if (activity.Mortality is not CaribouMortality)
        {
            throw new UnreachableException();
        }

        var mortalityIsPorcupine = huntedActivity.IsPorcupineCaribou();
        if (mortalityIsPorcupine)
        {
            return _isPorcupineHerd;
        }
        else
        {
            return true;
        }
    }
}

public class CaribouBagLimitEntryConfig : IEntityTypeConfiguration<CaribouBagLimitEntry>
{
    public void Configure(EntityTypeBuilder<CaribouBagLimitEntry> builder)
    {
        builder.ToTable(TableNameConstants.BagLimitEntries);
    }
}
