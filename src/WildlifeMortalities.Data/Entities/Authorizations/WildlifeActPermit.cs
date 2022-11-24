using WildlifeMortalities.Data.Entities.MortalityReports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class WildlifeActPermit : Authorization
{
    public string Conditions { get; set; }

    public override AuthorizationResult GetResult(MortalityReport report) =>
        throw new NotImplementedException();
}
