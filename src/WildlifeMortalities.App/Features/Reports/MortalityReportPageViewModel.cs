using System.Text.Json.Serialization;
using FluentValidation;
using WildlifeMortalities.Data;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Reports;

public class MortalityReportPageViewModel
{
    private DateTime? _dateSubmitted;

    public bool IsUpdate { get; }

    public MortalityReportPageViewModel()
    {
        IsUpdate = false;

        // Most reports are individual hunt, so we set it as the default
        ReportViewModel = new IndividualHuntedMortalityReportViewModel();
        ReportType = ReportType.IndividualHuntedMortalityReport;
    }

    public MortalityReportPageViewModel(ReportDetail reportDetail)
    {
        IsUpdate = true;
        switch (reportDetail.Report)
        {
            case IndividualHuntedMortalityReport individualHuntedMortalityReport:
                ReportType = ReportType.IndividualHuntedMortalityReport;
                ReportViewModel = new IndividualHuntedMortalityReportViewModel(
                    individualHuntedMortalityReport,
                    reportDetail
                );
                break;
            case OutfitterGuidedHuntReport outfitterGuidedHuntReport:
                ReportType = ReportType.OutfitterGuidedHuntReport;
                ReportViewModel = new OutfitterGuidedHuntReportViewModel(
                    outfitterGuidedHuntReport,
                    reportDetail
                );
                break;
            case SpecialGuidedHuntReport specialGuidedHuntReport:
                ReportType = ReportType.SpecialGuidedHuntReport;
                ReportViewModel = new SpecialGuidedHuntReportViewModel(
                    specialGuidedHuntReport,
                    reportDetail
                );
                break;
            case TrappedMortalitiesReport trappedMortalitiesReport:
                ReportType = ReportType.TrappedMortalitiesReport;
                ReportViewModel = new TrappedReportViewModel(
                    trappedMortalitiesReport,
                    reportDetail
                );
                break;
            default:
                throw new NotImplementedException();
        }
        DateSubmitted = reportDetail.Report.DateSubmitted.DateTime;
    }

    public ReportType ReportType { get; set; }

    public DateTime? DateSubmitted
    {
        get { return _dateSubmitted; }
        set
        {
            _dateSubmitted = value;
            ReportViewModel.DateSubmitted = value;
        }
    }

    [JsonConverter(typeof(MostConcreteClassJsonConverter<MortalityReportViewModel>))]
    public MortalityReportViewModel ReportViewModel { get; set; }
}

public class MortalityReportPageViewModelValidator : AbstractValidator<MortalityReportPageViewModel>
{
    public MortalityReportPageViewModelValidator()
    {
        RuleFor(x => x.ReportType).NotEmpty();

        RuleFor(x => x.ReportViewModel as IndividualHuntedMortalityReportViewModel)
            .SetValidator(new IndividualHuntedMortalityReportViewModelValidator())
            .When(x => x.ReportType == ReportType.IndividualHuntedMortalityReport);

        RuleFor(x => x.ReportViewModel as OutfitterGuidedHuntReportViewModel)
            .SetValidator(new OutfitterGuidedHuntReportViewModelValidator())
            .When(x => x.ReportType == ReportType.OutfitterGuidedHuntReport);

        RuleFor(x => x.ReportViewModel as SpecialGuidedHuntReportViewModel)
            .SetValidator(new SpecialGuidedHuntReportViewModelValidator())
            .When(x => x.ReportType == ReportType.SpecialGuidedHuntReport);

        RuleFor(x => x.ReportViewModel as TrappedReportViewModel)
            .SetValidator(new TrappedReportViewModelValidator())
            .When(x => x.ReportType == ReportType.TrappedMortalitiesReport);
    }
}
