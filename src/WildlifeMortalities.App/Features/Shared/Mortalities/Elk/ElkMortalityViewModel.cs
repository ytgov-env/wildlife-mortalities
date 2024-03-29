﻿using FluentValidation;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Shared.Extensions;
using WildlifeMortalities.Shared.Services;
using static WildlifeMortalities.Data.Entities.Mortalities.ElkMortality;

namespace WildlifeMortalities.App.Features.Shared.Mortalities.Elk;

public class ElkMortalityViewModel : MortalityViewModel
{
    public ElkMortalityViewModel()
        : base(Data.Enums.Species.Elk) { }

    public ElkMortalityViewModel(ElkMortality mortality, ReportDetail? reportDetail = null)
        : base(mortality, reportDetail)
    {
        Herd = mortality.Herd;
    }

    public ElkHerd? Herd { get; set; }

    public override Mortality GetMortality()
    {
        var mortality = new ElkMortality { Herd = Herd!.Value };
        SetBaseValues(mortality);

        return mortality;
    }

    public override Dictionary<string, string?> GetProperties()
    {
        var result = base.GetProperties();
        result.Add("Herd", Herd?.GetDisplayName());

        return result;
    }
}

public class ElkMortalityViewModelValidator : MortalityViewModelBaseValidator<ElkMortalityViewModel>
{
    public ElkMortalityViewModelValidator() => RuleFor(x => x.Herd).NotNull().IsInEnum();
}
