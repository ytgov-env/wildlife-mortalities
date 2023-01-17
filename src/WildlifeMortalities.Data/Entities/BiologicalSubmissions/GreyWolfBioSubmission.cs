using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class GreyWolfBioSubmission : BioSubmission<GreyWolfMortality>
{
    public PeltColour PeltColour { get; set; }
}

public enum PeltColour
{
    Black = 10,
    Brown = 20,
    Gray = 30,
    LightCream = 40
}

public class GreyWolfBioSubmissionConfig : IEntityTypeConfiguration<GreyWolfBioSubmission>
{
    public void Configure(EntityTypeBuilder<GreyWolfBioSubmission> builder) =>
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
}
