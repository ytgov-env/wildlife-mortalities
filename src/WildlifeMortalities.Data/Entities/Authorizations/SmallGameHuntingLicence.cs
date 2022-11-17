using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.MortalityReports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class SmallGameHuntingLicence : Authorization
{
    public LicenceType Type { get; set; }
    public enum LicenceType
    {
        Uninitialized = 0,
        CanadianResident,
        CanadianResidentFirstNationsOrInuit,
        NonResident,
        NonResidentFirstNationsOrInuit,
        YukonResident,
        YukonResidentSenior,
        YukonResidentFirstNationsOrInuit,
        YukonResidentFirstNationsOrInuitSenior
    }

    public override AuthorizationResult GetResult(MortalityReport report) => throw new NotImplementedException();
}

public class SmallGameHuntingLicenceConfig : IEntityTypeConfiguration<SmallGameHuntingLicence>
{
    public void Configure(EntityTypeBuilder<SmallGameHuntingLicence> builder)
    {
        builder.ToTable("Authorizations");
    }
}
