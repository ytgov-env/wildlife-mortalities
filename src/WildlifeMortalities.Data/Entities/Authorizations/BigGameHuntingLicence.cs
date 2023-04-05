using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class BigGameHuntingLicence : Authorization, IHasOutfitterAreas
{
    public enum LicenceType
    {
        [Display(Name = "Canadian resident")]
        CanadianResident = 10,

        [Display(Name = "Canadian resident - special guided")]
        CanadianResidentSpecialGuided = 20,

        [Display(Name = "Non-resident")]
        NonResident = 30,

        [Display(Name = "Yukon resident")]
        YukonResident = 40,

        [Display(Name = "Yukon resident 65+")]
        YukonResidentSenior = 50,

        [Display(Name = "Yukon resident - First Nations or Inuit")]
        YukonResidentFirstNationsOrInuit = 60,

        [Display(Name = "Yukon resident - First Nations or Inuit 65+")]
        YukonResidentFirstNationsOrInuitSenior = 70,

        [Display(Name = "Yukon resident trapper")]
        YukonResidentTrapper = 80
    }

    public BigGameHuntingLicence() { }

    public BigGameHuntingLicence(LicenceType type) => Type = type;

    public LicenceType Type { get; set; }
    public List<OutfitterArea> OutfitterAreas { get; set; } = null!;

    //public List<HuntingSeal> HuntingSeals { get; set; } = null!;
    //public List<HuntingPermit> HuntingPermits { get; set; } = null!;
    //public List<PhaHuntingPermit> PhaHuntingPermits { get; set; } = null!;
    //public SpecialGuideLicence? SpecialGuideLicence { get; set; }

    public override AuthorizationResult GetResult(Report report) =>
        throw new NotImplementedException();
}

public class BigGameHuntingLicenceConfig : IEntityTypeConfiguration<BigGameHuntingLicence>
{
    public void Configure(EntityTypeBuilder<BigGameHuntingLicence> builder) =>
        builder.ToTable("Authorizations");
}
