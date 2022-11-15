using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class BigGameHuntingLicence : Authorization
{
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

    public List<Seal> Seals { get; set; } = null!;
    public List<HuntingPermit> HuntingPermits { get; set; } = null!;
    public List<PhaHuntingPermit> PhaHuntingPermits { get; set; } = null!;
    public SpecialGuideLicence? SpecialGuideLicence { get; set; }
    public OutfitterAssistantGuideLicence? OutfitterAssistantGuideLicence { get; set; }
    public OutfitterChiefGuideLicence? OutfitterChiefGuideLicence { get; set; }
}

public class BigGameHuntingLicenceConfig : IEntityTypeConfiguration<BigGameHuntingLicence>
{
    public void Configure(EntityTypeBuilder<BigGameHuntingLicence> builder)
    {
    }
}
