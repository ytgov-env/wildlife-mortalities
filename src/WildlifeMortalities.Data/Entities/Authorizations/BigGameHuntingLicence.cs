using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class BigGameHuntingLicence : Authorization
{
    public enum LicenceType
    {
        CanadianResident = 10,
        CanadianResidentSpecialGuided = 20,
        NonResident = 30,
        YukonResident = 40,
        YukonResidentSenior = 50,
        YukonResidentFirstNationsOrInuit = 60,
        YukonResidentFirstNationsOrInuitSenior = 70,
        YukonResidentTrapper = 80
    }

    public BigGameHuntingLicence()
    {
    }

    public BigGameHuntingLicence(LicenceType type) => Type = type;

    public LicenceType Type { get; set; }

    public List<HuntingSeal> HuntingSeals { get; set; } = null!;
    public List<HuntingPermit> HuntingPermits { get; set; } = null!;
    public List<PhaHuntingPermit> PhaHuntingPermits { get; set; } = null!;
    public SpecialGuideLicence? SpecialGuideLicence { get; set; }
    public OutfitterAssistantGuideLicence? OutfitterAssistantGuideLicence { get; set; }
    public OutfitterChiefGuideLicence? OutfitterChiefGuideLicence { get; set; }

    public override AuthorizationResult GetResult(Report report) =>
        throw new NotImplementedException();
}

public class BigGameHuntingLicenceConfig : IEntityTypeConfiguration<BigGameHuntingLicence>
{
    public void Configure(EntityTypeBuilder<BigGameHuntingLicence> builder) =>
        builder.ToTable("Authorizations");
}
