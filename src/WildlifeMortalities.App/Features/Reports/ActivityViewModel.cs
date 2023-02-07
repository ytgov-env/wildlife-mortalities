using FluentValidation;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.App.Features.Shared.Mortalities;
using WildlifeMortalities.App.Features.Shared.Mortalities.AmericanBlackBear;
using WildlifeMortalities.App.Features.Shared.Mortalities.GrizzlyBear;
using WildlifeMortalities.App.Features.Shared.Mortalities.ThinhornSheep;
using WildlifeMortalities.App.Features.Shared.Mortalities.WoodBison;

namespace WildlifeMortalities.App.Features.Reports;

public class ActivityViewModel
{
    public bool IsCompleted { get; set; }
    public string Comment { get; set; } = string.Empty;

    public MortalityWithSpeciesSelectionViewModel MortalityWithSpeciesSelectionViewModel { get; set; } =
        new();
}

public class TrappedActivityViewModel : ActivityViewModel
{
    public TrappedActivity GetActivity()
    {
        throw new NotImplementedException();
    }
}

public class TrappedActivityViewModelValidator : AbstractValidator<TrappedActivityViewModel>
{
    public TrappedActivityViewModelValidator() { }
}

public class HuntedActivityViewModel : ActivityViewModel
{
    public string Landmark { get; set; } = string.Empty;
    public GameManagementArea? GameManagementArea { get; set; }

    public HuntedActivityViewModel() { }

    public HuntedActivityViewModel(
        HuntedActivity huntedActivity,
        IEnumerable<GameManagementArea> areas
    )
    {
        Landmark = huntedActivity.Landmark;
        Comment = huntedActivity.Comment;
        IsCompleted = true;
        GameManagementArea = areas.FirstOrDefault(x => x.Id == huntedActivity.GameManagementAreaId);
    }

    public HuntedActivity GetActivity() =>
        new()
        {
            Mortality = MortalityWithSpeciesSelectionViewModel.MortalityViewModel.GetMortality(),
            Landmark = Landmark,
            GameManagementAreaId = GameManagementArea?.Id ?? 0,
            Comment = Comment
        };
}

public class HuntedActivityViewModelValidator : AbstractValidator<HuntedActivityViewModel>
{
    public HuntedActivityViewModelValidator()
    {
        RuleFor(x => x.Landmark).NotNull();
        RuleFor(x => x.GameManagementArea).NotNull();
        RuleFor(x => x.Comment)
            .Length(10, 1000)
            .When(x => string.IsNullOrEmpty(x.Comment) == false);

        RuleFor(x => x.MortalityWithSpeciesSelectionViewModel.Species)
            .NotNull()
            .WithMessage("Please select a species");

        RuleFor(x => x.MortalityWithSpeciesSelectionViewModel.MortalityViewModel)
            .NotNull()
            .When(x => x.MortalityWithSpeciesSelectionViewModel.Species != null)
            .SetInheritanceValidator(x =>
            {
                x.Add(new AmericanBlackBearMortalityViewModelValidator());
                x.Add(new GrizzlyBearMortalityViewModelValidator());
                x.Add(new ThinhornSheepViewModelValidator());
                x.Add(new WoodBisonMortalityViewModelValidator());
                x.Add(new MortalityViewModelValidator());
            });
    }
}