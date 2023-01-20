using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class CanadaLynxBioSubmission : BioSubmission<CanadaLynxMortality>
{
    public int PeltLengthMillimetres { get; set; }
    public int PeltWidthMillimetres { get; set; }

    public CanadaLynxBioSubmission() { }

    public CanadaLynxBioSubmission(int mortalityId) : base(mortalityId) { }
}

public class CanadaLynxBioSubmissionConfig : IEntityTypeConfiguration<CanadaLynxBioSubmission>
{
    public void Configure(EntityTypeBuilder<CanadaLynxBioSubmission> builder) =>
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
}
