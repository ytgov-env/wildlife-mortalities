using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Users;

namespace WildlifeMortalities.Data.Entities.Reports;

public class DraftReport
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;
    public int CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;
    public DateTimeOffset DateCreated { get; set; }
    public int? LastModifiedById { get; set; }
    public User? LastModifiedBy { get; set; }
    public DateTimeOffset? DateLastModified { get; set; }
    public string SerializedData { get; set; } = string.Empty;
}

public class DraftReportConfig : IEntityTypeConfiguration<DraftReport>
{
    public void Configure(EntityTypeBuilder<DraftReport> builder) =>
        builder
            .Property(d => d.SerializedData)
            .HasColumnType("nvarchar(MAX)")
            .HasMaxLength(1_073_741_824);
}
