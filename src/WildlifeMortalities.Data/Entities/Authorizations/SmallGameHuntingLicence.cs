﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Extensions;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class SmallGameHuntingLicence : Authorization, IHasOutfitterAreas
{
    public enum LicenceType
    {
        [Display(Name = "Canadian resident")]
        CanadianResident = 10,

        [Display(Name = "Canadian resident - First Nations or Inuit")]
        CanadianResidentFirstNationsOrInuit = 20,

        [Display(Name = "Non-resident")]
        NonResident = 30,

        [Display(Name = "Non-resident - First Nations or Inuit")]
        NonResidentFirstNationsOrInuit = 40,

        [Display(Name = "Yukon resident")]
        YukonResident = 50,

        [Display(Name = "Yukon resident 65+")]
        YukonResidentSenior = 60,

        [Display(Name = "Yukon resident - First Nations or Inuit")]
        YukonResidentFirstNationsOrInuit = 70,

        [Display(Name = "Yukon resident - First Nations or Inuit 65+")]
        YukonResidentFirstNationsOrInuitSenior = 80
    }

    public SmallGameHuntingLicence() { }

    public SmallGameHuntingLicence(LicenceType type) => Type = type;

    [Column($"{nameof(SmallGameHuntingLicence)}_{nameof(Type)}")]
    public LicenceType Type { get; set; }
    public List<OutfitterArea> OutfitterAreas { get; set; } = null!;

    public override string GetAuthorizationType() =>
        $"Small game hunting licence - {Type.GetDisplayName().ToLower()}";

    protected override void UpdateInternal(Authorization authorization)
    {
        if (authorization is not SmallGameHuntingLicence smallGameHuntingLicence)
        {
            throw new ArgumentException(
                $"Expected {nameof(SmallGameHuntingLicence)} but received {authorization.GetType().Name}"
            );
        }

        Type = smallGameHuntingLicence.Type;
        OutfitterAreas = smallGameHuntingLicence.OutfitterAreas;
    }

    public override bool IsAYukonResident()
    {
        return Type
            is LicenceType.YukonResident
                or LicenceType.YukonResidentSenior
                or LicenceType.YukonResidentFirstNationsOrInuit
                or LicenceType.YukonResidentFirstNationsOrInuitSenior;
    }
}

public class SmallGameHuntingLicenceConfig : IEntityTypeConfiguration<SmallGameHuntingLicence>
{
    public void Configure(EntityTypeBuilder<SmallGameHuntingLicence> builder) =>
        builder.ToTable(TableNameConstants.Authorizations);
}
