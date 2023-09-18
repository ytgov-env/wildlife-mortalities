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

        ReportViewModel = new IndividualHuntedMortalityReportViewModel();
    }

    public MortalityReportPageViewModel(ReportDetail reportDetail)
    {
        IsUpdate = true;
        switch (reportDetail.Report)
        {
            case IndividualHuntedMortalityReport individualHuntedMortalityReport:
                SelectedReportType = ReportType.IndividualHuntedMortalityReport;
                ReportViewModel = new IndividualHuntedMortalityReportViewModel(
                    individualHuntedMortalityReport,
                    reportDetail
                );
                break;
            case OutfitterGuidedHuntReport outfitterGuidedHuntReport:
                SelectedReportType = ReportType.OutfitterGuidedHuntReport;
                ReportViewModel = new OutfitterGuidedHuntReportViewModel(
                    outfitterGuidedHuntReport,
                    reportDetail
                );
                break;
            case SpecialGuidedHuntReport specialGuidedHuntReport:
                SelectedReportType = ReportType.SpecialGuidedHuntReport;
                ReportViewModel = new SpecialGuidedHuntReportViewModel(
                    specialGuidedHuntReport,
                    reportDetail
                );
                break;
            case TrappedMortalitiesReport trappedMortalitiesReport:
                SelectedReportType = ReportType.TrappedMortalitiesReport;
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

    public ReportType? SelectedReportType { get; set; }

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
        RuleFor(x => x.SelectedReportType).NotEmpty();

        RuleFor(x => x.ReportViewModel as IndividualHuntedMortalityReportViewModel)
            .SetValidator(new IndividualHuntedMortalityReportViewModelValidator())
            .When(x => x.SelectedReportType == ReportType.IndividualHuntedMortalityReport);

        RuleFor(x => x.ReportViewModel as OutfitterGuidedHuntReportViewModel)
            .SetValidator(new OutfitterGuidedHuntReportViewModelValidator())
            .When(x => x.SelectedReportType == ReportType.OutfitterGuidedHuntReport);

        RuleFor(x => x.ReportViewModel as SpecialGuidedHuntReportViewModel)
            .SetValidator(new SpecialGuidedHuntReportViewModelValidator())
            .When(x => x.SelectedReportType == ReportType.SpecialGuidedHuntReport);

        RuleFor(x => x.ReportViewModel as TrappedReportViewModel)
            .SetValidator(new TrappedReportViewModelValidator())
            .When(x => x.SelectedReportType == ReportType.TrappedMortalitiesReport);
    }
}
