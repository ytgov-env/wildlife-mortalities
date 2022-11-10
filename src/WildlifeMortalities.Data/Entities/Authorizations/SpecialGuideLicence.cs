using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class SpecialGuideLicence : Authorization
{
    public int HuntingLicenceId { get; set; }
    public HuntingLicence HuntingLicence { get; set; } = default!;
}

public class SpecialGuideLicenceConfig : IEntityTypeConfiguration<SpecialGuideLicence>
{
    public void Configure(EntityTypeBuilder<SpecialGuideLicence> builder)
    {
        builder.HasOne(s => s.HuntingLicence)
            .WithOne(h => h.SpecialGuideLicence)
            .HasForeignKey<SpecialGuideLicence>(s=> s.HuntingLicenceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
