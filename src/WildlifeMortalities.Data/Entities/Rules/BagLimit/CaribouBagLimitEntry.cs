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

//public static class CaribouIsPorcupineResolver
//{
//    public static bool IsPorcupine(DateTimeOffset data, GameManagementArea area) => false;
//}

public class CaribouBagLimitEntry : HuntingBagLimitEntry
{
    //private bool _isPorcupineHerd = false;
    public bool IsPorcupineHerd => MaxValuePerPerson == 2;

    private CaribouBagLimitEntry() { }

    public CaribouBagLimitEntry(
        IEnumerable<GameManagementArea> areas,
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
        //bool? currentPorcupineValue = null;
        //foreach (var item in areas)
        //{
        //    var startIsPorcupineHerd = CaribouIsPorcupineResolver.IsPorcupine(PeriodStart, item);
        //    var endIsPorcupineHerd = CaribouIsPorcupineResolver.IsPorcupine(PeriodEnd, item);

        //    if (startIsPorcupineHerd != endIsPorcupineHerd)
        //    {
        //        throw new Exception();
        //    }

        //    if (currentPorcupineValue != null)
        //    {
        //        if (startIsPorcupineHerd != currentPorcupineValue)
        //        {
        //            throw new Exception();
        //        }
        //    }
        //    else
        //    {
        //        currentPorcupineValue = startIsPorcupineHerd;
        //    }
        //}
        //if (currentPorcupineValue == null)
        //{
        //    throw new UnreachableException();
        //}

        //_isPorcupineHerd = currentPorcupineValue.Value;
    }

    public override bool ShouldMutateBagValue(HarvestActivity activity)
    {
        if (activity is not HuntedActivity huntedActivity)
        {
            throw new UnreachableException();
        }

        if (activity.Mortality is not CaribouMortality caribouMortality)
        {
            throw new UnreachableException();
        }

        //var mortalityIsPorcupine = CaribouIsPorcupineResolver.IsPorcupine(
        //    caribouMortality.DateOfDeath!.Value,
        //    huntedActivity.GameManagementArea
        //);
        var isCaribouMortalityPorcupine = huntedActivity.GetLegalHerd() == CaribouHerd.Porcupine;
        if (isCaribouMortalityPorcupine)
        {
            return IsPorcupineHerd;
        }
        //if (mortalityIsPorcupine)
        //{
        //    return _isPorcupineHerd;
        //}
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
