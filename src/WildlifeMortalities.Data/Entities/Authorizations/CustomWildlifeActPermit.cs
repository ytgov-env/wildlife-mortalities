using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class CustomWildlifeActPermit : Authorization
{
    public string Conditions { get; set; }

    public override AuthorizationResult GetResult(Report report) =>
        throw new NotImplementedException();
}
