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
}
