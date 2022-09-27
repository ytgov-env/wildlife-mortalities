using FluentValidation;

namespace WildlifeMortalities.App.Features.MortalityReports;

public class MortalityReportViewModel
{
    public MortalityReportType MortalityReportType { get; set; } =
        MortalityReportType.IndividualHunt;

    public MortalityViewModel MortalityViewModel { get; set; } = new();
    public List<MortalityViewModel> MortalityViewModels { get; set; } = new();
    public string Landmark { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
}

public class MortalityReportViewModelValidator : AbstractValidator<MortalityReportViewModel>
{
    public MortalityReportViewModelValidator(MortalityReportType type)
    {
        RuleFor(x => x.Landmark).NotNull().When(x => type == MortalityReportType.IndividualHunt);
        RuleFor(x => x.Comment)
            .Length(10, 1000)
            .When(x => string.IsNullOrEmpty(x.Comment) == false);
    }
}
