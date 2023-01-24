using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class MountainGoatBioSubmission : BioSubmission<MountainGoatMortality>
{
    public MountainGoatBioSubmission()
    {
    }

    public MountainGoatBioSubmission(int mortalityId) : base(mortalityId)
    {
    }

    public bool IsHornIncluded { get; set; }
    public bool IsHeadIncluded { get; set; }
    public MountainGoatMortality Mortality { get; set; } = null!;
}

public class MountainGoatBioSubmissionConfig : IEntityTypeConfiguration<MountainGoatBioSubmission>
{
    public void Configure(EntityTypeBuilder<MountainGoatBioSubmission> builder) =>
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
}
