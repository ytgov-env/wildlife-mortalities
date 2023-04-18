using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class ElkMortality : Mortality, IHasBioSubmission
{
    public enum ElkHerd
    {
        Braeburn = 10,
        Takhini = 20
    }

    public ElkHerd Herd { get; set; }
    public override Species Species => Species.Elk;
    public ElkBioSubmission? BioSubmission { get; set; }

    public BioSubmission CreateDefaultBioSubmission() => new ElkBioSubmission(this);
}

public class ElkMortalityConfig : IEntityTypeConfiguration<ElkMortality>
{
    public void Configure(EntityTypeBuilder<ElkMortality> builder) =>
        builder
            .ToTable("Mortalities")
            .Property(w => w.Herd)
            .HasColumnName(nameof(ElkMortality.ElkHerd));
}
