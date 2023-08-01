using FluentValidation;
using WildlifeMortalities.App.Features.Shared.Mortalities;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Data.Extensions;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Reports;

public class HuntedActivityViewModel : ActivityViewModel
{
    private readonly ReportDetail? _reportDetail;

    public HuntedActivityViewModel() { }

    public HuntedActivityViewModel(HuntedActivity activity, ReportDetail? reportDetail = null)
        : base(activity, reportDetail)
    {
        HrbsNumber = activity.HrbsNumber;
        Seal = activity.Seal;
        Landmark = activity.Landmark;
        Comment = activity.Comment;
        _id = activity.Id;
        GameManagementArea = activity.GameManagementArea;
        _reportDetail = reportDetail;
    }

    public HuntedActivityViewModel(HuntedActivity activity, Report report)
        : this(activity, new ReportDetail(report, Array.Empty<(int, BioSubmission)>())) { }

    public HuntedActivityViewModel(HuntedActivityViewModel huntedActivityViewModel, Species species)
        : base(huntedActivityViewModel, species)
    {
        HrbsNumber = huntedActivityViewModel.HrbsNumber;
        Seal = huntedActivityViewModel.Seal;
        Landmark = huntedActivityViewModel.Landmark;
        GameManagementArea = huntedActivityViewModel.GameManagementArea;
    }

    public string HrbsNumber { get; set; } = string.Empty;
    public string Seal { get; set; } = string.Empty;
    public string Landmark { get; set; } = string.Empty;
    public GameManagementArea? GameManagementArea { get; set; }

    public HuntedActivity GetActivity()
    {
        var activity = new HuntedActivity()
        {
            Mortality = MortalityWithSpeciesSelectionViewModel.MortalityViewModel.GetMortality(),
            HrbsNumber = HrbsNumber,
            Seal = Seal,
            Landmark = Landmark,
            GameManagementAreaId = GameManagementArea?.Id ?? 0,
            Comment = Comment,
            Id = _id,
        };

        if (activity.Mortality is CaribouMortality caribouMortality)
        {
            caribouMortality.LegalHerd =
                GameManagementArea?.GetLegalHerd(caribouMortality.DateOfDeath!.Value)
                ?? CaribouMortality.CaribouHerd.Unknown;
        }

        if (_reportDetail != null)
        {
            switch (_reportDetail.Report)
            {
                case OutfitterGuidedHuntReport report:
                    activity.OutfitterGuidedHuntReportId = report.Id;
                    break;
                case IndividualHuntedMortalityReport report:
                    activity.IndividualHuntedMortalityReportId = report.Id;
                    break;
                case SpecialGuidedHuntReport report:
                    activity.SpecialGuidedHuntReportId = report.Id;
                    break;
                default:
                    throw new NotImplementedException("Report type not recognized.");
            }
        }

        return activity;
    }
}

public class HuntedActivityViewModelValidator : ActivityViewModelValidator<HuntedActivityViewModel>
{
    public HuntedActivityViewModelValidator()
    {
        RuleFor(x => x.HrbsNumber)
            .NotNull()
            .Matches(@"^\d{5}$")
            .WithMessage("HRBS number must be exactly 5 digits.");
        RuleFor(x => x.Seal)
            .NotNull()
            .Matches(@"^\d{4}$")
            .When(
                x =>
                    x.MortalityWithSpeciesSelectionViewModel.Species
                        is Species.AmericanBlackBear
                            or Species.Caribou
                            or Species.MuleDeer
                            or Species.Elk
                            or Species.GrizzlyBear
                            or Species.Moose
                            or Species.MountainGoat
                            or Species.ThinhornSheep
                            or Species.WoodBison
            )
            .WithMessage("Seal must be exactly 4 digits.");
        RuleFor(x => x.Landmark).NotNull();
        RuleFor(x => x.GameManagementArea).NotNull();
    }
}
