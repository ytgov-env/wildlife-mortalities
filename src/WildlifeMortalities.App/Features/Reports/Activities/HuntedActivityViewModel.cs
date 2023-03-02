using FluentValidation;
using WildlifeMortalities.App.Features.Shared.Mortalities;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Reports;

public class HuntedActivityViewModel : ActivityViewModel
{
    public HuntedActivityViewModel() { }

    public HuntedActivityViewModel(HuntedActivity activity, ReportDetail? reportDetail = null)
        : base(activity, reportDetail)
    {
        Landmark = activity.Landmark;
        Comment = activity.Comment;
        IsCompleted = true;
        GameManagementArea = activity.GameManagementArea;
    }

    public string Landmark { get; set; } = string.Empty;
    public GameManagementArea? GameManagementArea { get; set; }

    public HuntedActivity GetActivity() =>
        new()
        {
            Mortality = MortalityWithSpeciesSelectionViewModel.MortalityViewModel.GetMortality(),
            Landmark = Landmark,
            GameManagementAreaId = GameManagementArea?.Id ?? 0,
            Comment = Comment
        };
}

public class HuntedActivityViewModelValidator : ActivityViewModelValidator<HuntedActivityViewModel>
{
    public HuntedActivityViewModelValidator()
    {
        RuleFor(x => x.Landmark).NotNull();
        RuleFor(x => x.GameManagementArea).NotNull();
    }
}
