using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Mortalities;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

public class TrappedMortalitiesReport : Report, IHasClientReporter
{
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    public List<TrappedMortalityReport> TrappedMortalityReports { get; set; } = null!;

    public override IEnumerable<Mortality> GetMortalities() =>
        TrappedMortalityReports.Select(x => x.Mortality).ToArray();
}

public class TrappedMortalitiesReportConfig : IEntityTypeConfiguration<TrappedMortalitiesReport>
{
    public void Configure(EntityTypeBuilder<TrappedMortalitiesReport> builder) =>
        builder
            .ToTable("Reports")
            .HasOne(t => t.Client)
            .WithMany(t => t.TrappedMortalitiesReports)
            .OnDelete(DeleteBehavior.NoAction);
}
