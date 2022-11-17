using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.MortalityReports;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class TrappingLicence : Authorization
{
    public LicenceType Type { get; set; }
    public enum LicenceType
    {
        Uninitialized = 0,
        AssistantTrapper,
        AssistantTrapperSenior,
        ConcessionHolder,
        ConcessionHolderSenior,
        GroupConcessionAreaMember,
        GroupConcessionAreaMemberSenior
    }

    public override AuthorizationResult GetResult(MortalityReport report) => throw new NotImplementedException();
}

public class TrappingLicenceConfig : IEntityTypeConfiguration<TrappingLicence>
{
    public void Configure(EntityTypeBuilder<TrappingLicence> builder)
    {
        builder.ToTable("Authorizations");
    }
}
