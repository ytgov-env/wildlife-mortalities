using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Shared.Extensions;
using WildlifeMortalities.Shared.Services;
using static WildlifeMortalities.Data.Entities.Mortalities.CaribouMortality;

namespace WildlifeMortalities.App.Features.Shared.Mortalities.Caribou;

public class CaribouMortalityViewModel : MortalityViewModel
{
    public CaribouMortalityViewModel()
        : base(Data.Enums.Species.Caribou) { }

    public CaribouMortalityViewModel(CaribouMortality mortality, ReportDetail? reportDetail = null)
        : base(mortality, reportDetail)
    {
        LegalHerd = mortality.LegalHerd;
    }

    public CaribouHerd? LegalHerd { get; set; }

    public override Mortality GetMortality()
    {
        var mortality = new CaribouMortality();
        SetBaseValues(mortality);

        return mortality;
    }

    public override Dictionary<string, string?> GetProperties()
    {
        var result = base.GetProperties();
        result.Add("Herd", LegalHerd?.GetDisplayName());

        return result;
    }
}

public class CaribouMortalityViewModelValidator
    : MortalityViewModelBaseValidator<CaribouMortalityViewModel> { }
