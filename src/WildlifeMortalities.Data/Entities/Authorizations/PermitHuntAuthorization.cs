using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class PermitHuntAuthorization : Authorization
{
    public int HuntingLicenceId { get; set; }
    public HuntingLicence HuntingLicence { get; set; } = default!;
}

public class PermitHuntAuthorizationConfig : IEntityTypeConfiguration<PermitHuntAuthorization>
{
    public void Configure(EntityTypeBuilder<PermitHuntAuthorization> builder)
    {
        builder.HasOne(p => p.HuntingLicence)
            .WithMany(h => h.PermitHuntAuthorizations)
            .HasForeignKey(p => p.HuntingLicenceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
