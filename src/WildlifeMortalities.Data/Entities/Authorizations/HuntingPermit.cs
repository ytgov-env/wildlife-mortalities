﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Extensions;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class HuntingPermit : Authorization
{
    public enum PermitType
    {
        [Display(Name = "Fortymile caribou summer")]
        CaribouFortymileSummer = 10,

        [Display(Name = "Fortymile caribou winter")]
        CaribouFortymileWinter = 20,

        [Display(Name = "Hart river caribou")]
        CaribouHartRiver = 30,

        [Display(Name = "Nelchina caribou")]
        CaribouNelchina = 40,

        [Display(Name = "Elk adaptive")]
        ElkAdaptive = 50,

        [Display(Name = "Elk adaptive first nations")]
        ElkAdaptiveFirstNations = 60,

        [Display(Name = "Elk exclusion")]
        ElkExclusion = 70,

        [Display(Name = "Moose threshold")]
        MooseThreshold = 80,

        [Display(Name = "Bison")]
        WoodBison = 90
    }

    public HuntingPermit() { }

    public HuntingPermit(PermitType type) => Type = type;

    [Column($"{nameof(HuntingPermit)}_{nameof(Type)}")]
    public PermitType Type { get; set; }

    public bool IsCaribouRelated() =>
        Type
            is PermitType.CaribouFortymileSummer
                or PermitType.CaribouFortymileWinter
                or PermitType.CaribouHartRiver
                or PermitType.CaribouNelchina;

    public bool IsElkRelated() =>
        Type
            is PermitType.ElkExclusion
                or PermitType.ElkAdaptive
                or PermitType.ElkAdaptiveFirstNations;

    public bool IsMooseRelated() => Type is PermitType.MooseThreshold;

    public bool IsWoodBisonRelated() => Type is PermitType.WoodBison;

    public override string GetAuthorizationType() =>
        $"Hunting permit - {Type.GetDisplayName().ToLower()}";

    protected override void UpdateInternal(Authorization authorization)
    {
        if (authorization is not HuntingPermit huntingPermit)
        {
            throw new ArgumentException(
                $"Expected {nameof(HuntingPermit)} but received {authorization.GetType().Name}"
            );
        }

        Type = huntingPermit.Type;
    }
}

public class HuntingPermitConfig : IEntityTypeConfiguration<HuntingPermit>
{
    public void Configure(EntityTypeBuilder<HuntingPermit> builder) =>
        builder.ToTable(TableNameConstants.Authorizations);
}
