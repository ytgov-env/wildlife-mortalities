using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Extensions;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class TrappingLicence : Authorization
{
    public enum LicenceType
    {
        [Display(Name = "Assistant trapper")]
        AssistantTrapper = 10,

        [Display(Name = "Assistant trapper 65+")]
        AssistantTrapperSenior = 20,

        [Display(Name = "Concession holder")]
        ConcessionHolder = 30,

        [Display(Name = "Concession holder 65+")]
        ConcessionHolderSenior = 40,

        [Display(Name = "Group concession area member")]
        GroupConcessionAreaMember = 50,

        [Display(Name = "Group concession area member 65+")]
        GroupConcessionAreaMemberSenior = 60
    }

    public TrappingLicence() { }

    public TrappingLicence(LicenceType type) => Type = type;

    [Column($"{nameof(TrappingLicence)}_{nameof(Type)}")]
    public LicenceType Type { get; set; }

    [Column($"{nameof(TrappingLicence)}_{nameof(RegisteredTrappingConcessionId)}")]
    public int RegisteredTrappingConcessionId { get; set; }
    public RegisteredTrappingConcession RegisteredTrappingConcession { get; set; } = null!;

    public override AuthorizationResult GetResult(Report report) =>
        throw new NotImplementedException();

    public override string GetAuthorizationType() =>
        $"Trapping licence - {Type.GetDisplayName().ToLower()}";

    protected override void UpdateInternal(Authorization authorization)
    {
        if (authorization is not TrappingLicence trappingLicence)
        {
            throw new ArgumentException(
                $"Expected {nameof(TrappingLicence)} but received {authorization.GetType().Name}"
            );
        }

        Type = trappingLicence.Type;
        RegisteredTrappingConcession = trappingLicence.RegisteredTrappingConcession;
    }
}

public class TrappingLicenceConfig : IEntityTypeConfiguration<TrappingLicence>
{
    public void Configure(EntityTypeBuilder<TrappingLicence> builder) =>
        builder.ToTable(TableNameConstants.Authorizations);
}
