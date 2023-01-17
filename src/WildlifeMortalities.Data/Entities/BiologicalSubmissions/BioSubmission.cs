using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public abstract class BioSubmission
{
    public int Id { get; set; }
    public Age Age { get; set; } = null!;
}

public abstract class BioSubmission<T> : BioSubmission where T : Mortality
{
    public int MortalityId { get; set; }

    public T Mortality { get; set; }
}

public class BioSubmissionConfig : IEntityTypeConfiguration<BioSubmission>
{
    public void Configure(EntityTypeBuilder<BioSubmission> builder) => builder.OwnsOne(b => b.Age);
}

public class Age
{
    public int Years { get; set; }
    public ConfidenceInAge Confidence { get; set; }
}

public enum ConfidenceInAge
{
    Fair = 10,
    Good = 20,
    Poor = 30
}
