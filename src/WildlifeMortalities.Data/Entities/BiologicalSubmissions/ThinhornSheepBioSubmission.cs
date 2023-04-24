using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class ThinhornSheepBioSubmission
    : BioSubmission<ThinhornSheepMortality>,
        IHasHornMeasurementEntries
{
    public ThinhornSheepBioSubmission() { }

    public ThinhornSheepBioSubmission(ThinhornSheepMortality mortality)
        : base(mortality) { }

    [IsRequiredOrganicMaterialForBioSubmission("Horns")]
    public bool? IsHornsProvided { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Head")]
    public bool? IsHeadProvided { get; set; }

    public int? HornLengthToThirdAnnulusMillimetres { get; set; }
    public bool? IsFullCurl { get; set; }
    public string? PlugNumber { get; set; }

    public HornMeasured? HornMeasured { get; set; }
    public BroomedStatus? BroomedStatus { get; set; }

    public int? HornTotalLengthMillimetres { get; set; }
    public int? HornBaseCircumferenceMillimetres { get; set; }
    public int? HornTipSpreadMillimetres { get; set; }

    public List<HornMeasurementEntry> HornMeasurementEntries { get; set; } = null!;

    public override bool CanBeAnalysed => true;

    public override bool HasSubmittedAllRequiredOrganicMaterial() =>
        IsHornsProvided == true && IsHeadProvided == true;
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
        builder
            .HasIndex(x => x.MortalityId)
            .HasFilter($"[{nameof(ThinhornSheepBioSubmission)}_MortalityId] IS NOT NULL");
    }
}
