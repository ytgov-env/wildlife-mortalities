using FluentValidation;
using WildlifeMortalities.App.Features.Shared.Mortalities;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.MortalityReports;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.MortalityReports;

public class MortalityReportPageViewModel
{
    public MortalityReportType MortalityReportType { get; set; } =
        MortalityReportType.IndividualHunt;

    public HuntedMortalityReportViewModel? HuntedMortalityReportViewModel { get; set; } = new();

    public OutfitterGuidedHuntReportViewModel? OutfitterGuidedHuntReportViewModel { get; set; }
    public SpecialGuidedHuntReportViewModel? SpecialGuidedHuntReportViewModel { get; set; }
}

public class MortalityReportPageViewModelValidator : AbstractValidator<MortalityReportPageViewModel>
{
    public MortalityReportPageViewModelValidator()
    {
        RuleFor(x => x.MortalityReportType).NotEmpty();

        RuleFor(x => x.HuntedMortalityReportViewModel)
            .SetValidator(new HuntedMortalityReportViewModelValidator())
            .When(x => x.MortalityReportType == MortalityReportType.IndividualHunt);
    }
}

public class MortalityWithSpeciesSelectionViewModel
{
    public AllSpecies? Species { get; set; }
    public MortalityViewModel MortalityViewModel { get; set; }
}

public class HuntedMortalityReportViewModel
{
    public string Landmark { get; set; } = string.Empty;
    public GameManagementArea? GameManagementArea { get; set; }
    public string Comment { get; set; } = string.Empty;

    public MortalityWithSpeciesSelectionViewModel MortalityWithSpeciesSelectionViewModel { get; set; } = new();

    public HuntedMortalityReport GetReport(int personId)
    {
        var species = GameManagementArea.ResolveSubType(MortalityWithSpeciesSelectionViewModel.Species!.Value);

        var report = new HuntedMortalityReport
        {
            Mortality = MortalityWithSpeciesSelectionViewModel.MortalityViewModel.GetMortality(species),
            Landmark = Landmark,
            GameManagementAreaId = GameManagementArea.Id,
            Comment = Comment,
            ClientId = personId
        };

        return report;
    }
}

public class HuntedMortalityReportViewModelValidator
    : AbstractValidator<HuntedMortalityReportViewModel>
{
    public HuntedMortalityReportViewModelValidator()
    {
        RuleFor(x => x.Landmark).NotNull();
        RuleFor(x => x.GameManagementArea).NotNull();
        RuleFor(x => x.Comment)
            .Length(10, 1000)
            .When(x => string.IsNullOrEmpty(x.Comment) == false);

        RuleFor(x => x.MortalityWithSpeciesSelectionViewModel.Species).NotNull();

        RuleFor(x => x.MortalityWithSpeciesSelectionViewModel.MortalityViewModel)
            .NotNull()
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
