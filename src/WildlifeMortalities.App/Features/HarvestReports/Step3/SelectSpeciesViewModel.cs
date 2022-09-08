using FluentValidation;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.HarvestReports;

public class SelectSpeciesViewModel
{
    public TrappedSpecies? TrappedSpecies { get; set; }
    public HuntedSpecies? HuntedSpecies { get; set; }
}

public class SelectSpeciesViewModelValidator : AbstractValidator<SelectSpeciesViewModel>
{
    public SelectSpeciesViewModelValidator(HarvestReportType type)
    {
        RuleFor(x => x.TrappedSpecies).NotNull().When(_ => type == HarvestReportType.Trapping);
        RuleFor(x => x.HuntedSpecies).NotNull().When(_ => type == HarvestReportType.Hunting);
    }
}
