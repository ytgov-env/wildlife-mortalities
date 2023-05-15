using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class CaribouBioSubmission : BioSubmission<CaribouMortality>
{
    public CaribouBioSubmission() { }

    public CaribouBioSubmission(CaribouMortality mortality)
        : base(mortality) { }

    [IsRequiredOrganicMaterialForBioSubmission("Incisor bar")]
    [Column($"{nameof(CaribouBioSubmission)}_{nameof(IsIncisorBarProvided)}")]
    public bool? IsIncisorBarProvided { get; set; }
}

public class CaribouBioSubmissionConfig : IEntityTypeConfiguration<CaribouBioSubmission>
{
    public void Configure(EntityTypeBuilder<CaribouBioSubmission> builder)
    {
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder
            .Property(x => x.MortalityId)
            .HasColumnName($"{builder.Metadata.ClrType.Name}_MortalityId");
    }
}
