using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using WildlifeMortalities.Data.Entities.Reporters;
using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.App.Features.HarvestReports;

public class SelectReporterViewModel
{
    public ClientDto SelectedClient { get; set; }
    public ConservationOfficer SelectedConservationOfficer { get; set; }
    public ReporterType ReporterType { get; set; }
}

public class SelectReporterViewModelValidator : AbstractValidator<SelectReporterViewModel>
{
    public SelectReporterViewModelValidator()
    {
        RuleFor(x => x.SelectedConservationOfficer)
            .NotNull()
            .When(x => x.ReporterType == ReporterType.ConservationOfficer);

        RuleFor(x => x.SelectedClient).NotNull().When(x => x.ReporterType == ReporterType.Client);
    }
}

public enum ReporterType
{
    Client,
    ConservationOfficer
}
