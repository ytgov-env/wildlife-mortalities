using WildlifeMortalities.App.Features.Shared.Mortalities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Reports;

public class TrappedActivityViewModel : ActivityViewModel
{
    public TrappedActivityViewModel() { }

    public TrappedActivityViewModel(TrappedActivity activity, ReportDetail? reportDetail = null)
        : base(activity, reportDetail) { }

    public TrappedActivityViewModel(TrappedActivity activity, TrappedMortalitiesReport report)
        : this(activity, new ReportDetail(report, Array.Empty<(int, BioSubmission)>())) { }

    public TrappedActivity GetActivity() =>
        new()
        {
            Mortality = MortalityWithSpeciesSelectionViewModel.MortalityViewModel.GetMortality(),
            Comment = Comment
        };
}

public class TrappedActivityViewModelValidator
    : ActivityViewModelValidator<TrappedActivityViewModel> { }
