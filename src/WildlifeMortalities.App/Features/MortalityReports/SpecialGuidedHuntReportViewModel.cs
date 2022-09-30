using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.App.Features.MortalityReports;

public class SpecialGuidedHuntReportViewModel
{
    public ClientDto Guide { get; set; } = null!;
    public GuidedHuntResult? Result { get; set; }
    public List<HuntedMortalityReportViewModel> HuntedMortalityReportViewModels { get; set; } =
        new List<HuntedMortalityReportViewModel>();
}

public class SpecialGuidedHuntReportViewModelValidator
    : AbstractValidator<SpecialGuidedHuntReportViewModel>
{
    public SpecialGuidedHuntReportViewModelValidator() { }
}
