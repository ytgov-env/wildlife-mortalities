using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class ResearchActivity : Activity
{
    [Column($"{nameof(ResearchActivity)}_{nameof(ReportId)}")]
    public int ReportId { get; set; }
    public ResearchMortalityReport Report { get; set; } = null!;
}

public class ResearchActivityConfig : IEntityTypeConfiguration<ResearchActivity>
{
    public void Configure(EntityTypeBuilder<ResearchActivity> builder) =>
        builder
            .ToTable(TableNameConstants.Activities)
            .HasOne(x => x.Report)
            .WithOne(x => x.Activity)
            .OnDelete(DeleteBehavior.NoAction);
}
