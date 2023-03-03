using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public abstract class BioSubmission
{
    public int Id { get; set; }
    public string Comment { get; set; } = string.Empty;
    public Age? Age { get; set; }
    public List<UploadFileInfo> UploadFileInfo { get; set; } = null!;

    public abstract void ClearDependencies();
}

public abstract class BioSubmission<T> : BioSubmission
    where T : Mortality
{
    protected BioSubmission() { }

    protected BioSubmission(int mortalityId) => MortalityId = mortalityId;

    public int MortalityId { get; set; }

    public T Mortality { get; set; }

    public override void ClearDependencies() => Mortality = null!;
}

public class BioSubmissionConfig : IEntityTypeConfiguration<BioSubmission>
{
    public void Configure(EntityTypeBuilder<BioSubmission> builder)
    {
        builder.OwnsOne(
            b => b.Age,
            a =>
            {
                a.Property(a => a.Confidence).IsRequired();
                a.WithOwner();
            }
        );
        builder.OwnsMany(
            b => b.UploadFileInfo,
            ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            }
        );
    }
}

public class Age
{
    public int Years { get; set; }
    public ConfidenceInAge? Confidence { get; set; }
}

public class UploadFileInfo
{
    public string PathToFile { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
}

public enum ConfidenceInAge
{
    [Display(Name = "Fair")]
    Fair = 10,

    [Display(Name = "Good")]
    Good = 20,

    [Display(Name = "Poor")]
    Poor = 30
}
