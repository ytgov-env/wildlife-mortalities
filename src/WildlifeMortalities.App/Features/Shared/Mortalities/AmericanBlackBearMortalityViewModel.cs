using FluentValidation;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.App.Features.MortalityReports;

public class AmericanBlackBearMortalityViewModel : MortalityViewModel
{
    public bool IsShotInConflict { get; set; }

    public AmericanBlackBearMortalityViewModel() : base(Data.Enums.AllSpecies.AmericanBlackBear) { }

    public override Mortality GetMortality()
    {
        var mortality = new AmericanBlackBearMortality { IsShotInConflict = IsShotInConflict, };
        SetBaseValues(mortality);

        return mortality;
    }

    public override Dictionary<string, string> GetProperties()
    {
        var result = base.GetProperties();
        result.Add("Is Shot in conflict", IsShotInConflict.ToString());

        return result;
    }
}

public class AmericanBlackBearMortalityViewModelValidator
    : MortalityViewModelBaseValidator<AmericanBlackBearMortalityViewModel>
{
    public AmericanBlackBearMortalityViewModelValidator() : base()
    {
        RuleFor(x => x.IsShotInConflict).Equal(true);
    }
}
