using FluentValidation;

namespace WildlifeMortalities.App.Features.MortalityReports;

public class MortalityReportViewModel
{
    public MortalityReportType MortalityReportType { get; set; } = MortalityReportType.Hunted;

    public MortalityViewModel MortalityViewModel { get; set; } = new();
    public List<MortalityViewModel> MortalityViewModels { get; set; } = new();
    public string Landmark { get; set; } = string.Empty;
    public string Comments { get; set; } = string.Empty;
}

public class MortalityReportViewModelValidator : AbstractValidator<MortalityReportViewModel>
{
    public MortalityReportViewModelValidator(MortalityReportType type)
    {
        RuleFor(x => x.Landmark).NotNull().When(x => type == MortalityReportType.Hunted);
        RuleFor(x => x.Comments)
            .Length(10, 1000)
            .When(x => string.IsNullOrEmpty(x.Comments) == false);
    }
}
