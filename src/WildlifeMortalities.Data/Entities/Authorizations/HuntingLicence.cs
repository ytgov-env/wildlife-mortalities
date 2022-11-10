using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class HuntingLicence : Authorization
{
    public List<Seal> Seals { get; set; } = null!;
    public List<PermitHuntAuthorization> PermitHuntAuthorizations { get; set; }
    public SpecialGuideLicence? SpecialGuideLicence { get; set; }
}

public class HuntingLicenceConfig : IEntityTypeConfiguration<HuntingLicence>
{
    public void Configure(EntityTypeBuilder<HuntingLicence> builder)
    {
    }
}
