using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Users;

namespace WildlifeMortalities.Data.Entities.Reports;

public class DeletedReport
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;
    public string HumanReadableId { get; set; } = string.Empty;
    public Season Season { get; set; } = null!;
    public DateTimeOffset? DateLastModified { get; set; }
    public DateTimeOffset DateSubmitted { get; set; }
    public DateTimeOffset DateDeleted { get; set; }
    public string Reason { get; set; } = string.Empty;
    public int DeletedById { get; set; }
    public User DeletedBy { get; set; } = null!;
    public string SerializedData { get; set; } = string.Empty;
}

public class DeletedReportConfig : IEntityTypeConfiguration<DeletedReport>
{
    public void Configure(EntityTypeBuilder<DeletedReport> builder)
    {
        builder.ToTable(Constants.TableNameConstants.DeletedReports);
        builder
            .Property(d => d.SerializedData)
            .HasColumnType("nvarchar(MAX)")
            .HasMaxLength(1_073_741_824);
    }
}
