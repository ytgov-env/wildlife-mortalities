﻿using FluentValidation;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.Reports;

public abstract class MortalityReportViewModel
{
    public int ReportId { get; } = Constants.EfCore.TemporaryAutoGeneratedKey;

    public DateTime? DateSubmitted { get; set; }
    public string HumanReadableId { get; }

    protected MortalityReportViewModel()
    {
        DateSubmitted = DateTime.Now.Date;
    }

    protected MortalityReportViewModel(Report report)
    {
        DateSubmitted = report.DateSubmitted.Date;
        HumanReadableId = report.HumanReadableId;
        ReportId = report.Id;
    }

    protected void SetReportBaseValues(Report report)
    {
        report.DateSubmitted = DateSubmitted ?? DateTime.Now.Date;
    }

    public abstract Report GetReport(int personId);

    internal abstract void SpeciesChanged(int id, Species species);
}

public abstract class MortalityReportViewModelValidator<T> : AbstractValidator<T>
    where T : MortalityReportViewModel
{
    protected MortalityReportViewModelValidator()
    {
        RuleFor(x => x.DateSubmitted).NotNull();
        RuleFor(x => x.DateSubmitted)
            .Must(x => x <= DateTime.Now.Date)
            .WithMessage("Date submitted cannot be in the future.");
    }
}
