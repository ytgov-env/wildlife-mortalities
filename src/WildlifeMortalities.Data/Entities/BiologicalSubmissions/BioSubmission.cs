using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public abstract class BioSubmission
{
    public int Id { get; set; }
    public Age Age { get; set; } = null!;
}

public class BioSubmissionConfig : IEntityTypeConfiguration<BioSubmission>
{
    public void Configure(EntityTypeBuilder<BioSubmission> builder) =>
        builder.OwnsOne(b => b.Age, a => a.ToTable("Age"));
}

public class Age
{
    public int Years { get; set; }
    public ConfidenceInAge Confidence { get; set; }
}

public enum ConfidenceInAge
{
    Uninitialized = 0,
    Fair = 1,
    Good = 2,
    Poor = 3
}
