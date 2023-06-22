using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Mortalities;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class WoodBisonBioSubmission : BioSubmission<WoodBisonMortality>
{
    public WoodBisonBioSubmission() { }

    public WoodBisonBioSubmission(WoodBisonMortality mortality)
        : base(mortality) { }

    [IsRequiredOrganicMaterialForBioSubmission("Incisor bar")]
    [Column($"{nameof(WoodBisonBioSubmission)}_{nameof(IsIncisorBarProvided)}")]
    public bool? IsIncisorBarProvided { get; set; }
}

public class WoodBisonBioSubmissionConfig : IEntityTypeConfiguration<WoodBisonBioSubmission>
{
    public void Configure(EntityTypeBuilder<WoodBisonBioSubmission> builder)
    {
        builder
            .ToTable(TableNameConstants.BioSubmissions)
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder
            .Property(x => x.MortalityId)
            .HasColumnName($"{builder.Metadata.ClrType.Name}_MortalityId");
    }
}
