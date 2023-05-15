using System.ComponentModel.DataAnnotations.Schema;
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
    [IsPrerequisiteOrganicMaterialForBioSubmissionAnalysis]
    [Column($"{nameof(ThinhornSheepBioSubmission)}_{nameof(IsHornsProvided)}")]
    public bool? IsHornsProvided { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Head")]
    [IsPrerequisiteOrganicMaterialForBioSubmissionAnalysis]
    [Column($"{nameof(ThinhornSheepBioSubmission)}_{nameof(IsHeadProvided)}")]
    public bool? IsHeadProvided { get; set; }

    [Column($"{nameof(ThinhornSheepBioSubmission)}_{nameof(HornLengthToThirdAnnulusMillimetres)}")]
    public int? HornLengthToThirdAnnulusMillimetres { get; set; }

    [Column($"{nameof(ThinhornSheepBioSubmission)}_{nameof(IsFullCurl)}")]
    public bool? IsFullCurl { get; set; }

    [Column($"{nameof(ThinhornSheepBioSubmission)}_{nameof(PlugNumber)}")]
    public string? PlugNumber { get; set; }

    [Column($"{nameof(ThinhornSheepBioSubmission)}_{nameof(HornMeasured)}")]
    public HornMeasured? HornMeasured { get; set; }

    [Column($"{nameof(ThinhornSheepBioSubmission)}_{nameof(BroomedStatus)}")]
    public BroomedStatus? BroomedStatus { get; set; }

    [Column($"{nameof(ThinhornSheepBioSubmission)}_{nameof(HornTotalLengthMillimetres)}")]
    public int? HornTotalLengthMillimetres { get; set; }

    [Column($"{nameof(ThinhornSheepBioSubmission)}_{nameof(HornBaseCircumferenceMillimetres)}")]
    public int? HornBaseCircumferenceMillimetres { get; set; }

    [Column($"{nameof(ThinhornSheepBioSubmission)}_{nameof(HornTipSpreadMillimetres)}")]
    public int? HornTipSpreadMillimetres { get; set; }

    public List<HornMeasurementEntry> HornMeasurementEntries { get; set; } = null!;

    public override bool CanBeAnalysed => true;
}

public class ThinhornSheepBioSubmissionConfig : IEntityTypeConfiguration<ThinhornSheepBioSubmission>
{
    public void Configure(EntityTypeBuilder<ThinhornSheepBioSubmission> builder)
    {
        builder
            .ToTable("BioSubmissions")
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder.OwnsMany(
            t => t.HornMeasurementEntries,
            ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.Ignore(h => h.IsBroomed);
                ownedNavigationBuilder.ToJson("ThinhornSheepBioSubmission_HornMeasurementEntries");
            }
        );
        builder
            .Property(x => x.MortalityId)
            .HasColumnName($"{builder.Metadata.ClrType.Name}_MortalityId");
    }
}
