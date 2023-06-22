using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class AmericanBlackBearMortality : Mortality<AmericanBlackBearMortality>, IHasBioSubmission
{
    [Column($"{nameof(AmericanBlackBearMortality)}_{nameof(IsShotInConflict)}")]
    public bool IsShotInConflict { get; set; }
    public AmericanBlackBearBioSubmission? BioSubmission { get; set; }

    public override Species Species => Species.AmericanBlackBear;

    public BioSubmission CreateDefaultBioSubmission() => new AmericanBlackBearBioSubmission(this);

    public override IEntityTypeConfiguration<AmericanBlackBearMortality> GetConfig() =>
        new AmericanBlackBearMortalityConfig();
}

public class AmericanBlackBearMortalityConfig : IEntityTypeConfiguration<AmericanBlackBearMortality>
{
    public void Configure(EntityTypeBuilder<AmericanBlackBearMortality> builder) =>
        builder.ToTable(TableNameConstants.Mortalities);
}
