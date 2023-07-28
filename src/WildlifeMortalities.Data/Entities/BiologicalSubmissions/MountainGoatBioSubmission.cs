using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using WildlifeMortalities.Data.Entities.Mortalities;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public class MountainGoatBioSubmission : BioSubmission<MountainGoatMortality>
{
    public MountainGoatBioSubmission() { }

    public MountainGoatBioSubmission(MountainGoatMortality mortality)
        : base(mortality) { }

    [IsRequiredOrganicMaterialForBioSubmission("Horns")]
    [IsPrerequisiteOrganicMaterialForBioSubmissionAnalysis]
    [Column($"{nameof(MountainGoatBioSubmission)}_{nameof(IsHornsProvided)}")]
    public bool? IsHornsProvided { get; set; }

    [IsRequiredOrganicMaterialForBioSubmission("Head")]
    [IsPrerequisiteOrganicMaterialForBioSubmissionAnalysis]
    [Column($"{nameof(MountainGoatBioSubmission)}_{nameof(IsHeadProvided)}")]
    public bool? IsHeadProvided { get; set; }

    [Column($"{nameof(MountainGoatBioSubmission)}_{nameof(HornMeasured)}")]
    public HornMeasured? HornMeasured { get; set; }

    [Column($"{nameof(MountainGoatBioSubmission)}_{nameof(HornTotalLengthMillimetres)}")]
    public int? HornTotalLengthMillimetres { get; set; }

    [Column($"{nameof(MountainGoatBioSubmission)}_{nameof(HornBaseCircumferenceMillimetres)}")]
    public int? HornBaseCircumferenceMillimetres { get; set; }

    [Column($"{nameof(MountainGoatBioSubmission)}_{nameof(HornTipSpreadMillimetres)}")]
    public int? HornTipSpreadMillimetres { get; set; }
    public List<MountainGoatHornMeasurementEntry> HornMeasurementEntries { get; set; } = null!;

    public override bool CanBeAnalysed => true;
}

public class MountainGoatBioSubmissionConfig : IEntityTypeConfiguration<MountainGoatBioSubmission>
{
    public void Configure(EntityTypeBuilder<MountainGoatBioSubmission> builder)
    {
        builder
            .ToTable(TableNameConstants.BioSubmissions)
            .HasOne(b => b.Mortality)
            .WithOne(m => m.BioSubmission)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder.OwnsMany(
            t => t.HornMeasurementEntries,
            ownedNavigationBuilder =>
                ownedNavigationBuilder.ToJson("MountainGoatBioSubmission_HornMeasurementEntries")
        );
        builder
            .Property(x => x.MortalityId)
            .HasColumnName($"{builder.Metadata.ClrType.Name}_MortalityId");
    }
}

public class MountainGoatHornMeasurementEntry
{
    public int Annulus { get; set; }
    public int LengthMillimetres { get; set; }
}
