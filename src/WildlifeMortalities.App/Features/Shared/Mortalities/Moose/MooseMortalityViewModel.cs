using FluentValidation;
using WildlifeMortalities.App.Features.Shared.Mortalities.WoodBison;
using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Shared.Mortalities.Moose;

public class MooseMortalityViewModel : MortalityViewModel
{
    public MooseMortalityViewModel()
        : base(Data.Enums.Species.Moose) { }

    public MooseMortalityViewModel(MooseMortality mortality, ReportDetail? reportDetail = null)
        : base(mortality, reportDetail)
    {
        GameManagementSubArea = mortality.GameManagementSubArea;
    }

    public GameManagementSubArea? GameManagementSubArea { get; set; }

    public override Mortality GetMortality()
    {
        var mortality = new MooseMortality { GameManagementSubArea = GameManagementSubArea };

        SetBaseValues(mortality);
        return mortality;
    }

    public override Dictionary<string, string?> GetProperties()
    {
        var result = base.GetProperties();
        result.Add("Sub area", GameManagementSubArea?.SubArea);

        return result;
    }
}

public class MooseMortalityViewModelValidator
    : MortalityViewModelBaseValidator<MooseMortalityViewModel>
{
    public MooseMortalityViewModelValidator() =>
        RuleFor(x => x.GameManagementSubArea).NotNull().When(x.);
}
