using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Extensions;
using static WildlifeMortalities.Data.Constants;

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

    [Column($"{nameof(BigGameHuntingLicence)}_{nameof(Type)}")]
    public LicenceType Type { get; set; }
    public List<OutfitterArea> OutfitterAreas { get; set; } = null!;

    public override string GetAuthorizationType() =>
        $"Big game hunting licence - {Type.GetDisplayName().ToLower()}";

    protected override void UpdateInternal(Authorization authorization)
    {
        if (authorization is not BigGameHuntingLicence bigGameHuntingLicence)
        {
            throw new ArgumentException(
                $"Expected {nameof(BigGameHuntingLicence)} but received {authorization.GetType().Name}"
            );
        }

        Type = bigGameHuntingLicence.Type;
        OutfitterAreas = bigGameHuntingLicence.OutfitterAreas;
    }
}

public class BigGameHuntingLicenceConfig : IEntityTypeConfiguration<BigGameHuntingLicence>
{
    public void Configure(EntityTypeBuilder<BigGameHuntingLicence> builder) =>
        builder.ToTable(TableNameConstants.Authorizations);
}
