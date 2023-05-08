using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class MountainGoatBioSubmission
    : BioSubmission<MountainGoatMortality>,
        IHasHornMeasurementEntries
{
    public MountainGoatBioSubmission() { }

    public MountainGoatBioSubmission(MountainGoatMortality mortality)
        : base(mortality) { }

    [IsRequiredOrganicMaterialForBioSubmission("Horns")]
    [IsPrerequisiteOrganicMaterialForBioSubmissionAnalysis]
    public bool? IsHornsProvided { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Head")]
    [IsPrerequisiteOrganicMaterialForBioSubmissionAnalysis]
    public bool? IsHeadProvided { get; set; }

    public HornMeasured? HornMeasured { get; set; }
    public BroomedStatus? BroomedStatus { get; set; }

    public int? HornTotalLengthMillimetres { get; set; }
    public int? HornBaseCircumferenceMillimetres { get; set; }
    public int? HornTipSpreadMillimetres { get; set; }
    public List<HornMeasurementEntry> HornMeasurementEntries { get; set; } = null!;

    public override bool CanBeAnalysed => true;
}

public class MountainGoatBioSubmissionConfig : IEntityTypeConfiguration<MountainGoatBioSubmission>
{
    public void Configure(EntityTypeBuilder<MountainGoatBioSubmission> builder)
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
                ownedNavigationBuilder.ToJson("MountainGoatBioSubmission_HornMeasurementEntries");
            }
        );
        builder
            .HasIndex(x => x.MortalityId)
            .HasFilter($"[{nameof(MountainGoatBioSubmission)}_MortalityId] IS NOT NULL");
    }
}
