using WildlifeMortalities.Data.Entities;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Shared.Validators;

public class TrappedHarvestReportValidator<T> : AbstractValidator<TrappedHarvestReport>
    where T : Mortality { }
