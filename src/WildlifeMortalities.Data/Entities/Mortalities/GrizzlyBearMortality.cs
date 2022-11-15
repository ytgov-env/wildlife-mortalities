using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class GrizzlyBearMortality : Mortality<GrizzlyBearMortality>
{
    public bool IsShotInConflict { get; set; }

    public override IEntityTypeConfiguration<GrizzlyBearMortality> GetConfig() =>
        new GrizzlyBearMortalityConfig();
}

public class GrizzlyBearMortalityConfig : IEntityTypeConfiguration<GrizzlyBearMortality>
{
    public void Configure(EntityTypeBuilder<GrizzlyBearMortality> builder)
    {
        builder
            .Property(m => m.IsShotInConflict)
            .HasColumnName(nameof(GrizzlyBearMortality.IsShotInConflict));
    }
}
