using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Reports;

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

    public LicenceType Type { get; set; }
    public int RegisteredTrappingConcessionId { get; set; }
    public RegisteredTrappingConcession RegisteredTrappingConcession { get; set; } = null!;

    public override AuthorizationResult GetResult(Report report) =>
        throw new NotImplementedException();
}

public class TrappingLicenceConfig : IEntityTypeConfiguration<TrappingLicence>
{
    public void Configure(EntityTypeBuilder<TrappingLicence> builder) =>
        builder.ToTable("Authorizations");
}
