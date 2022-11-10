
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class HuntingPermit : Authorization
{
    public int SealId { get; set; }
    public Seal Seal { get; set; } = default!;
}

public class HuntingPermitConfig : IEntityTypeConfiguration<HuntingPermit>
{
    public void Configure(EntityTypeBuilder<HuntingPermit> builder)
    {
        builder.HasOne(h => h.Seal)
            .WithMany(s => s.HuntingPermits)
            .HasForeignKey(h => h.SealId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
