using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class AmericanBlackBearMortality : Mortality<AmericanBlackBearMortality>
{
    public bool IsShotInConflict { get; set; }

    public override IEntityTypeConfiguration<AmericanBlackBearMortality> GetConfig() =>
        new AmericanBlackBearMortalityConfig();
}

public class AmericanBlackBearMortalityConfig : IEntityTypeConfiguration<AmericanBlackBearMortality>
{
    public void Configure(EntityTypeBuilder<AmericanBlackBearMortality> builder) =>
        builder
            .Property(m => m.IsShotInConflict)
            .HasColumnName(nameof(AmericanBlackBearMortality.IsShotInConflict));
}
