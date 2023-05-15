using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class GreyWolfBioSubmission : BioSubmission<GreyWolfMortality>, IHasFurbearerSeal
{
    public GreyWolfBioSubmission() { }

    public GreyWolfBioSubmission(GreyWolfMortality mortality)
        : base(mortality) { }

    [Column($"{nameof(GreyWolfBioSubmission)}_{nameof(PeltColour)}")]
    public GreyWolfPeltColour? PeltColour { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Pelt")]
    [IsPrerequisiteOrganicMaterialForBioSubmissionAnalysis]
    [Column($"{nameof(GreyWolfBioSubmission)}_{nameof(IsPeltProvided)}")]
    public bool? IsPeltProvided { get; set; }

    [Column($"{nameof(FurbearerSealNumber)}")]
    public string? FurbearerSealNumber { get; set; }
    public override bool CanBeAnalysed => true;

    public enum GreyWolfPeltColour
    {
        [Display(Name = "Black")]
        Black = 10,

        [Display(Name = "Brown")]
        Brown = 20,

        [Display(Name = "Gray")]
        Gray = 30,

        [Display(Name = "Light cream")]
        LightCream = 40
    }
}

public class GreyWolfBioSubmissionConfig : IEntityTypeConfiguration<GreyWolfBioSubmission>
{
    public void Configure(EntityTypeBuilder<GreyWolfBioSubmission> builder)
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
