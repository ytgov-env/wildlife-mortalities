using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Extensions;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class HuntingSeal : Authorization
{
    public enum SealType
    {
        [Display(Name = SpeciesConstants.AmericanBlackBear)]
        AmericanBlackBear = 10,

        [Display(Name = SpeciesConstants.Caribou)]
        Caribou = 20,

        [Display(Name = SpeciesConstants.MuleDeer)]
        MuleDeer = 30,

        [Display(Name = SpeciesConstants.Elk)]
        Elk = 40,

        [Display(Name = SpeciesConstants.GrizzlyBear)]
        GrizzlyBear = 50,

        [Display(Name = SpeciesConstants.Moose)]
        Moose = 60,

        [Display(Name = SpeciesConstants.MountainGoat)]
        MountainGoat = 70,

        [Display(Name = SpeciesConstants.ThinhornSheep)]
        ThinhornSheep = 80,

        [Display(Name = SpeciesConstants.WoodBison)]
        WoodBison = 90
    }

    public HuntingSeal() { }

    public HuntingSeal(SealType type) => Type = type;

    [Column($"{nameof(HuntingSeal)}_{nameof(Type)}")]
    public SealType Type { get; set; }

    [Column($"{nameof(HuntingSeal)}_{nameof(HuntedActivityId)}")]
    public int? HuntedActivityId { get; set; }
    public HuntedActivity? HuntedActivity { get; set; }

    public override AuthorizationResult GetResult(Report report) =>
        throw new NotImplementedException();

    public override string GetAuthorizationType() =>
        $"Hunting seal - {Type.GetDisplayName().ToLower()}";

    protected override void UpdateInternal(Authorization authorization)
    {
        if (authorization is not HuntingSeal huntingSeal)
        {
            throw new ArgumentException(
                $"Expected {nameof(HuntingSeal)} but received {authorization.GetType().Name}"
            );
        }

        Type = huntingSeal.Type;
    }
}

public class SealConfig : IEntityTypeConfiguration<HuntingSeal>
{
    public void Configure(EntityTypeBuilder<HuntingSeal> builder) =>
        builder.ToTable(TableNameConstants.Authorizations);
}
