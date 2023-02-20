using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class GreyWolfBioSubmission : BioSubmission<GreyWolfMortality>
{
    public GreyWolfBioSubmission() { }

    public GreyWolfBioSubmission(int mortalityId) : base(mortalityId) { }

    public PeltColour? PeltColour { get; set; }
}

public enum PeltColour
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

public class GreyWolfBioSubmissionConfig : IEntityTypeConfiguration<GreyWolfBioSubmission>
{
    public void Configure(EntityTypeBuilder<GreyWolfBioSubmission> builder)
    {
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Property(g => g.PeltColour).IsRequired();
        builder
            .HasIndex(x => x.MortalityId)
            .HasFilter("[GreyWolfBioSubmission_MortalityId] IS NOT NULL");
    }
}
