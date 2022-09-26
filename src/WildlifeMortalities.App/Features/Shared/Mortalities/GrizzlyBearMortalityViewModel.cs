using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.App.Features.MortalityReports;

public class GrizzlyBearMortalityViewModel : MortalityViewModel
{
    public bool IsShotInConflict { get; set; }

    public GrizzlyBearMortalityViewModel() : base(Data.Enums.AllSpecies.GrizzlyBear) { }

    public override Mortality GetMortality()
    {
        var mortality = new GrizzlyBearMortality { IsShotInConflict = IsShotInConflict, };
        SetBaseValues(mortality);

        return mortality;
    }

    public override Dictionary<string, string> GetProperties()
    {
        var result = base.GetProperties();
        result.Add("Was shot in conflict", IsShotInConflict.ToString());

        return result;
    }
}
