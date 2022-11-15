using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
}

public class SmallGameHuntingLicenceConfig : IEntityTypeConfiguration<SmallGameHuntingLicence>
{
    public void Configure(EntityTypeBuilder<SmallGameHuntingLicence> builder)
    {
    }
}
