using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions;
using WildlifeMortalities.Data.Entities.BiologicalSubmissions.Shared;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Mortalities;

public class ElkMortality : Mortality, IHasBioSubmission
{
    public enum ElkHerd
    {
        [Display(Name = "Braeburn")]
        Braeburn = 10,

        [Display(Name = "Takhini")]
        Takhini = 20
    }

    [Column($"{nameof(ElkMortality)}_{nameof(Herd)}")]
    public ElkHerd Herd { get; set; }
    public override Species Species => Species.Elk;
    public ElkBioSubmission? BioSubmission { get; set; }

    public BioSubmission CreateDefaultBioSubmission() => new ElkBioSubmission(this);
}

public class ElkMortalityConfig : IEntityTypeConfiguration<ElkMortality>
{
    public void Configure(EntityTypeBuilder<ElkMortality> builder) =>
        builder.ToTable(TableNameConstants.Mortalities);
}
