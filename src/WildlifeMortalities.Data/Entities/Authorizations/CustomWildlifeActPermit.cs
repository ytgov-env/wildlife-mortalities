using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class CustomWildlifeActPermit : Authorization
{
    public string Conditions { get; set; }

    public override AuthorizationResult GetResult(MortalityReport report) =>
        throw new NotImplementedException();
}
