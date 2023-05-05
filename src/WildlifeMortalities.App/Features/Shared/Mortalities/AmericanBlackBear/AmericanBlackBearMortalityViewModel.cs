using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Shared.Mortalities.AmericanBlackBear;

public class AmericanBlackBearMortalityViewModel : MortalityViewModel
{
    public AmericanBlackBearMortalityViewModel()
        : base(Data.Enums.Species.AmericanBlackBear) { }

    public AmericanBlackBearMortalityViewModel(
        AmericanBlackBearMortality mortality,
        ReportDetail? reportDetail = null
    )
        : base(mortality, reportDetail)
    {
        IsShotInConflict = mortality.IsShotInConflict;
    }

    public bool IsShotInConflict { get; set; }

    public override Mortality GetMortality()
    {
        var mortality = new AmericanBlackBearMortality { IsShotInConflict = IsShotInConflict };
        SetBaseValues(mortality);

        return mortality;
    }

    public override Dictionary<string, string?> GetProperties()
    {
        var result = base.GetProperties();
        result.Add("Was shot in conflict", IsShotInConflict.ToString());

        return result;
    }
}

public class AmericanBlackBearMortalityViewModelValidator
    : MortalityViewModelBaseValidator<AmericanBlackBearMortalityViewModel> { }
