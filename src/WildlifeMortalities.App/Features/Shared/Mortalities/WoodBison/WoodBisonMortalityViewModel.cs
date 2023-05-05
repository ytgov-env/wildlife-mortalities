using FluentValidation;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Shared.Extensions;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Shared.Mortalities.WoodBison;

public class WoodBisonMortalityViewModel : MortalityViewModel
{
    public WoodBisonMortalityViewModel()
        : base(Data.Enums.Species.WoodBison) { }

    public WoodBisonMortalityViewModel(
        WoodBisonMortality mortality,
        ReportDetail? reportDetail = null
    )
        : base(mortality, reportDetail)
    {
        PregnancyStatus = mortality.PregnancyStatus;
        IsWounded = mortality.IsWounded;
    }

    public PregnancyStatus? PregnancyStatus { get; set; }
    public bool IsWounded { get; set; }

    public override Mortality GetMortality()
    {
        var mortality = new WoodBisonMortality
        {
            PregnancyStatus = PregnancyStatus,
            IsWounded = IsWounded
        };

        SetBaseValues(mortality);
        return mortality;
    }

    public override Dictionary<string, string?> GetProperties()
    {
        var result = base.GetProperties();
        result.Add("Was pregnant", PregnancyStatus?.GetDisplayName());
        result.Add("Was wounded", IsWounded.ToString());

        return result;
    }
}

public class WoodBisonMortalityViewModelValidator
    : MortalityViewModelBaseValidator<WoodBisonMortalityViewModel>
{
    public WoodBisonMortalityViewModelValidator() =>
        RuleFor(x => x.PregnancyStatus)
            .NotNull()
            .IsInEnum()
            .When(x => x.Sex is Data.Enums.Sex.Female);
}
