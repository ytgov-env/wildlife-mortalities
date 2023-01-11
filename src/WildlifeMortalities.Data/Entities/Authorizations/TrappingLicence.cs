using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.MortalityReports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class TrappingLicence : Authorization
{
    public enum LicenceType
    {
        AssistantTrapper = 10,
        AssistantTrapperSenior = 20,
        ConcessionHolder = 30,
        ConcessionHolderSenior = 40,
        GroupConcessionAreaMember = 50,
        GroupConcessionAreaMemberSenior = 60
    }

    public TrappingLicence()
    {
    }

    public TrappingLicence(LicenceType type) => Type = type;

    public LicenceType Type { get; set; }

    public override AuthorizationResult GetResult(MortalityReport report) =>
        throw new NotImplementedException();
}

public class TrappingLicenceConfig : IEntityTypeConfiguration<TrappingLicence>
{
    public void Configure(EntityTypeBuilder<TrappingLicence> builder) =>
        builder.ToTable("Authorizations");
}
