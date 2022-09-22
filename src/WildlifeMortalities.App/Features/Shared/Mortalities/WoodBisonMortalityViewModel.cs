using FluentValidation;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.App.Features.MortalityReports;

public class WoodBisonMortalityViewModel : MortalityViewModel
{
    public PregnancyStatus? PregnancyStatus { get; set; }
    public bool IsWounded { get; set; }

    public WoodBisonMortalityViewModel() : base(Data.Enums.AllSpecies.WoodBison) { }

    public override Mortality GetMortality()
    {
        var mortality = new WoodBisonMortality
        {
            PregnancyStatus = PregnancyStatus!.Value,
            IsWounded = IsWounded
        };

        SetBaseValues(mortality);
        return mortality;
    }
}

public class WoodBisonMortalityViewModelValidator : AbstractValidator<WoodBisonMortalityViewModel>
{
    public WoodBisonMortalityViewModelValidator()
    {
        RuleFor(x => x.PregnancyStatus).NotNull();
    }
}
