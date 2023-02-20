using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.App.Features.Shared.Mortalities.GrizzlyBear;

public class GrizzlyBearMortalityViewModel : MortalityViewModel
{
    public GrizzlyBearMortalityViewModel() : base(Data.Enums.Species.GrizzlyBear)
    {
    }

    public bool IsShotInConflict { get; set; }

    public override Mortality GetMortality()
    {
        var mortality = new GrizzlyBearMortality { IsShotInConflict = IsShotInConflict };
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

public class GrizzlyBearMortalityViewModelValidator
    : MortalityViewModelBaseValidator<GrizzlyBearMortalityViewModel>
{
}
