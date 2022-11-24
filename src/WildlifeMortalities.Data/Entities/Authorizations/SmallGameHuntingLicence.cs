using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.MortalityReports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class SmallGameHuntingLicence : Authorization
{
    public SmallGameHuntingLicence()
    {
    }

    public SmallGameHuntingLicence(LicenceType type)
    {
        Type = type;
    }

    public LicenceType Type { get; set; }

    public enum LicenceType
    {
        CanadianResident = 10,
        CanadianResidentFirstNationsOrInuit = 20,
        NonResident = 30,
        NonResidentFirstNationsOrInuit = 40,
        YukonResident = 50,
        YukonResidentSenior = 60,
        YukonResidentFirstNationsOrInuit = 70,
        YukonResidentFirstNationsOrInuitSenior = 80
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
