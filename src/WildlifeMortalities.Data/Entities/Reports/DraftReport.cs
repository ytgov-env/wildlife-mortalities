using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.Reports;

public class DraftReport
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;
    public DateTimeOffset DateLastModified { get; set; }
    public DateTimeOffset DateSubmitted { get; set; }
    public string SerializedData { get; set; } = string.Empty;
}

public class DraftReportConfig : IEntityTypeConfiguration<DraftReport>
{
    public void Configure(EntityTypeBuilder<DraftReport> builder) =>
        builder.Property(d => d.SerializedData).HasColumnType("nvarchar(4000)").HasMaxLength(4000);
}
