using WildlifeMortalities.Data.Entities.Authorizations;

namespace WildlifeMortalities.App.Features.Reporters;

public class AuthorizationsViewModel
{
    public IEnumerable<Authorization> Authorizations { get; set; } = default!;
}
