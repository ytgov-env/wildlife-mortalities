using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public class TrappedMortalityReport : MortalityReport /*, IHasClientReporter*/
{
    public int TrappedMortalitiesReportId { get; set; }
    public TrappedMortalitiesReport TrappedMortalitiesReport { get; set; } = null!;
    public int TrappingConcessionAreaId { get; set; }
    public RegisteredTrappingConcession RegisteredTrappingConcession { get; set; } = null!;
    public DateTimeOffset DateStarted { get; set; }
    public DateTimeOffset DateCompleted { get; set; }
    public int? AuthorizationId { get; set; }
    public Authorization? Authorization { get; set; }
    public TrappedMortalityReportStatus Status { get; set; }
    public string Comment { get; set; } = string.Empty;

    public List<Violation> Violations { get; set; } = null!;
    //public int ClientId { get; set; }
    //public Client Client { get; set; } = null!;
}

public enum TrappedMortalityReportStatus
{
}

public class TrappedMortalityReportConfig : IEntityTypeConfiguration<TrappedMortalityReport>
{
    public void Configure(EntityTypeBuilder<TrappedMortalityReport> builder) =>
        builder
            .ToTable("Reports")
            .HasOne(t => t.TrappedMortalitiesReport)
            .WithMany(t => t.TrappedMortalityReports)
            .OnDelete(DeleteBehavior.NoAction);
}
