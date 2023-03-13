using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class SmallGameHuntingLicence : Authorization, IHasOutfitterAreas
{
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

    public SmallGameHuntingLicence() { }

    public SmallGameHuntingLicence(LicenceType type) => Type = type;

    public LicenceType Type { get; set; }
    public List<OutfitterArea> OutfitterAreas { get; set; } = null!;

    public override AuthorizationResult GetResult(Report report) =>
        throw new NotImplementedException();
}

public class SmallGameHuntingLicenceConfig : IEntityTypeConfiguration<SmallGameHuntingLicence>
{
    public void Configure(EntityTypeBuilder<SmallGameHuntingLicence> builder) =>
        builder.ToTable("Authorizations");
}
