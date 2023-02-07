using FluentValidation;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.App.Features.Shared.Mortalities.ThinhornSheep;

public class ThinhornSheepMortalityViewModel : MortalityViewModel
{
    public ThinhornSheepMortalityViewModel() : base(Data.Enums.Species.ThinhornSheep) { }

    public ThinhornSheepBodyColour? BodyColour { get; set; }
    public ThinhornSheepTailColour? TailColour { get; set; }

    public override Mortality GetMortality()
    {
        var mortality = new ThinhornSheepMortality
        {
            BodyColour = BodyColour!.Value,
            TailColour = TailColour!.Value
        };

        SetBaseValues(mortality);
        return mortality;
    }

    public override Dictionary<string, string?> GetProperties()
    {
        var result = base.GetProperties();
        result.Add("Body colour", BodyColour?.ToString());
        result.Add("Tail colour", TailColour?.ToString());

        return result;
    }
}

public class ThinhornSheepViewModelValidator : AbstractValidator<ThinhornSheepMortalityViewModel>
{
    public ThinhornSheepViewModelValidator()
    {
        RuleFor(x => x.BodyColour).NotNull();
        RuleFor(x => x.TailColour).NotNull();
    }
}
