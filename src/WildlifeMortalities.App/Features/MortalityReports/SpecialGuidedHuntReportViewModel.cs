using FluentValidation;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.App.Features.MortalityReports;

public class SpecialGuidedHuntReportViewModel
{
    public ClientDto Guide { get; set; } = null!;
    public GuidedHuntResult? Result { get; set; }

    public List<HuntedMortalityReportViewModel> HuntedMortalityReportViewModels { get; set; } = new();
}

public class SpecialGuidedHuntReportViewModelValidator
    : AbstractValidator<SpecialGuidedHuntReportViewModel>
{
}
