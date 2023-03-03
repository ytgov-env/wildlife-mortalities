using FluentValidation;
using WildlifeMortalities.App.Extensions;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.App.Features.Shared.Mortalities.ThinhornSheep;

public class ThinhornSheepMortalityViewModel : MortalityViewModel
{
    private ThinhornSheepBodyColour? _bodyColour;

    public ThinhornSheepMortalityViewModel()
        : base(Data.Enums.Species.ThinhornSheep) { }

    public ThinhornSheepBodyColour? BodyColour
    {
        get { return _bodyColour; }
        set
        {
            _bodyColour = value;
            // Fannin sheep always have a dark tail
            if (value == ThinhornSheepBodyColour.Fannin)
            {
                TailColour = ThinhornSheepTailColour.Dark;
            }
        }
    }
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
        result.Add("Body colour", BodyColour?.GetDisplayName());
        result.Add("Tail colour", TailColour?.GetDisplayName());

        return result;
    }
}

public class ThinhornSheepViewModelValidator
    : MortalityViewModelBaseValidator<ThinhornSheepMortalityViewModel>
{
    public ThinhornSheepViewModelValidator()
    {
        RuleFor(x => x.BodyColour).NotNull().IsInEnum();
        RuleFor(x => x.TailColour).NotNull().IsInEnum();
        RuleFor(x => x.TailColour)
            .Equal(ThinhornSheepTailColour.Dark)
            .When(x => x.BodyColour == ThinhornSheepBodyColour.Fannin)
            .WithMessage(
                $"Tail colour must be {nameof(ThinhornSheepBodyColour.Dark)} when body colour is {nameof(ThinhornSheepBodyColour.Fannin)}."
            );
    }
}
