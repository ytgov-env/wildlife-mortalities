using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class ThinhornSheepBioSubmission
    : BioSubmission<ThinhornSheepMortality>,
        IHasHornMeasurementEntries
{
    public ThinhornSheepBioSubmission() { }

    public ThinhornSheepBioSubmission(int mortalityId) : base(mortalityId) { }

    public int? HornLengthToThirdAnnulusMillimetres { get; set; }
    public string PlugNumber { get; set; } = string.Empty;

    public HornMeasured? HornMeasured { get; set; }
    public BroomedStatus? BroomedStatus { get; set; }

    public int? HornTotalLengthMillimetres { get; set; }
    public int? HornBaseCircumferenceMillimetres { get; set; }
    public int? HornTipSpreadMillimetres { get; set; }

    public List<HornMeasurementEntry> HornMeasurementEntries { get; set; } = null!;
}

public class ThinhornSheepBioSubmissionConfig : IEntityTypeConfiguration<ThinhornSheepBioSubmission>
{
    public void Configure(EntityTypeBuilder<ThinhornSheepBioSubmission> builder)
    {
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.NoAction);
        builder.OwnsMany(
            t => t.HornMeasurementEntries,
            ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.Ignore(h => h.IsBroomed);
                ownedNavigationBuilder.ToJson("ThinhornSheepBioSubmission_HornMeasurementEntries");
            }
        );
        builder.Property(t => t.HornMeasured).IsRequired();
        builder.Property(t => t.BroomedStatus).IsRequired();
        builder
            .HasIndex(x => x.MortalityId)
            .HasFilter("[ThinhornSheepBioSubmission_MortalityId] IS NOT NULL");
    }
}
