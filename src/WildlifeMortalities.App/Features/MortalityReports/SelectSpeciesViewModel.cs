using FluentValidation;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.MortalityReports;

public class SelectSpeciesViewModel
{
    public TrappedSpecies? TrappedSpecies { get; set; }
    public HuntedSpecies? HuntedSpecies { get; set; }
}

public class SelectSpeciesViewModelValidator : AbstractValidator<SelectSpeciesViewModel>
{
    public SelectSpeciesViewModelValidator(MortalityReportType type)
    {
        RuleFor(x => x.TrappedSpecies).NotNull().When(_ => type == MortalityReportType.Trapped);
        RuleFor(x => x.HuntedSpecies).NotNull().When(_ => type == MortalityReportType.Hunted);
    }
}
