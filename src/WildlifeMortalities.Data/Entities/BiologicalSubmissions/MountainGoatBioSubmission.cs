using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class MountainGoatBioSubmission
    : BioSubmission<MountainGoatMortality>,
        IHasHornMeasurementEntries
{
    public MountainGoatBioSubmission() { }

    public MountainGoatBioSubmission(int mortalityId) : base(mortalityId) { }

    public HornMeasured? HornMeasured { get; set; }
    public BroomedStatus? BroomedStatus { get; set; }

    public int? HornTotalLengthMillimetres { get; set; }
    public int? HornBaseCircumferenceMillimetres { get; set; }
    public int? HornTipSpreadMillimetres { get; set; }
    public List<HornMeasurementEntry> HornMeasurementEntries { get; set; } = null!;

    public bool IsHornIncluded { get; set; }
    public bool IsHeadIncluded { get; set; }
}

public class MountainGoatBioSubmissionConfig : IEntityTypeConfiguration<MountainGoatBioSubmission>
{
    public void Configure(EntityTypeBuilder<MountainGoatBioSubmission> builder)
    {
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
        builder.OwnsMany(
            t => t.HornMeasurementEntries,
            ownedNavigationBuilder =>
                ownedNavigationBuilder.ToJson("MountainGoatBioSubmission_HornMeasurementEntries")
        );
        builder.Property(m => m.HornMeasured).IsRequired();
        builder.Property(m => m.BroomedStatus).IsRequired();
    }
}
