using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.MortalityReports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class BigGameHuntingLicence : Authorization
{
    public BigGameHuntingLicence()
    {

    }

    public BigGameHuntingLicence(LicenceType type)
    {
        Type = type;
    }
    public LicenceType Type { get; set; }

    public enum LicenceType
    {
        Uninitialized = 0,
        CanadianResident,
        CanadianResidentSpecialGuided,
        NonResident,
        YukonResident,
        YukonResidentSenior,
        YukonResidentFirstNationsOrInuit,
        YukonResidentFirstNationsOrInuitSenior,
        YukonResidentTrapper
    }

    public List<HuntingSeal> HuntingSeals { get; set; } = null!;
    public List<HuntingPermit> HuntingPermits { get; set; } = null!;
    public List<PhaHuntingPermit> PhaHuntingPermits { get; set; } = null!;
    public SpecialGuideLicence? SpecialGuideLicence { get; set; }
    public OutfitterAssistantGuideLicence? OutfitterAssistantGuideLicence { get; set; }
    public OutfitterChiefGuideLicence? OutfitterChiefGuideLicence { get; set; }
    public override AuthorizationResult GetResult(MortalityReport report) => throw new NotImplementedException();
}

public class BigGameHuntingLicenceConfig : IEntityTypeConfiguration<BigGameHuntingLicence>
{
    public void Configure(EntityTypeBuilder<BigGameHuntingLicence> builder)
    {
        builder.ToTable("Authorizations");
    }
}
