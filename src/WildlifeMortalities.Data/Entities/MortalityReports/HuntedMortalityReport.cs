using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities.MortalityReports;

public class HuntedMortalityReport : MortalityReport
{
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    public int GmaSpeciesId { get; set; }
    public GameManagementAreaSpecies GmaSpecies { get; set; } = null!;
    public DateTimeOffset DateStarted { get; set; }
    public DateTimeOffset DateCompleted { get; set; }
    public string Landmark { get; set; } = string.Empty;
    public int? SealId { get; set; }
    public Seal? Seal { get; set; }
    public HuntedMortalityReportStatus Status { get; set; }
    public string Comment { get; set; } = string.Empty;
    public List<Violation> Violations { get; set; } = null!;
}

public class HuntedMortalityReportConfig : IEntityTypeConfiguration<HuntedMortalityReport>
{
    public void Configure(EntityTypeBuilder<HuntedMortalityReport> builder) => builder.Property(h => h.Status).HasConversion<string>();
}
