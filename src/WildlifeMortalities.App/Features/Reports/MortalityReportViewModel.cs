using FluentValidation;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.App.Features.Reports;

public abstract class MortalityReportViewModel
{
    public DateTimeOffset? DateSubmitted { get; set; }

    protected MortalityReportViewModel() { }

    protected MortalityReportViewModel(Report report)
    {
        DateSubmitted = report.DateSubmitted;
    }

    protected void SetReportBaseValues(Report report)
    {
        report.DateSubmitted = DateSubmitted ?? DateTimeOffset.Now;
    }

    public abstract Report GetReport(int personId);

    internal abstract void SpeciesChanged(int id, Species species);
}

public abstract class MortalityReportViewModelValidator<T> : AbstractValidator<T>
    where T : MortalityReportViewModel
{
    protected MortalityReportViewModelValidator() { }
}
