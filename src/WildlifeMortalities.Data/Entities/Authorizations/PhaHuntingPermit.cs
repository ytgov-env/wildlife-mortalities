using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Extensions;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class PhaHuntingPermit : Authorization
{
    public enum PermitType
    {
        [Display(Name = "Caribou")]
        Caribou = 10,

        [Display(Name = "Deer")]
        Deer = 20,

        [Display(Name = "Elk")]
        Elk = 30,

        [Display(Name = "Goat")]
        MountainGoat = 40,

        [Display(Name = "Moose")]
        Moose = 50,

        [Display(Name = "Sheep")]
        ThinhornSheep = 60,

        [Display(Name = "Kluane sheep")]
        ThinhornSheepKluane = 70
    }

    public PhaHuntingPermit() { }

    public PhaHuntingPermit(PermitType type) => Type = type;

    [Column($"{nameof(PhaHuntingPermit)}_{nameof(Type)}")]
    public PermitType Type { get; set; }

    [Column($"{nameof(PhaHuntingPermit)}_{nameof(HuntCode)}")]
    public string HuntCode { get; set; } = string.Empty;

    public override AuthorizationResult GetResult(Report report) =>
        throw new NotImplementedException();

    public override string GetAuthorizationType() =>
        $"Pha hunting permit - {Type.GetDisplayName().ToLower()}";

    protected override void UpdateInternal(Authorization authorization)
    {
        if (authorization is not PhaHuntingPermit phaHuntingPermit)
        {
            throw new ArgumentException(
                $"Expected {nameof(PhaHuntingPermit)} but received {authorization.GetType().Name}"
            );
        }

        Type = phaHuntingPermit.Type;
        HuntCode = phaHuntingPermit.HuntCode;
    }
}

public class PhaHuntingPermitConfig : IEntityTypeConfiguration<PhaHuntingPermit>
{
    public void Configure(EntityTypeBuilder<PhaHuntingPermit> builder) =>
        builder.ToTable(TableNameConstants.Authorizations);
}
