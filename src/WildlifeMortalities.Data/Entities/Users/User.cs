using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Reports;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Users;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;

    public string NameIdentifier { get; set; } = string.Empty;
    public UserSettings Settings { get; set; } = null!;
    public List<Report> CreatedReports { get; set; } = null!;
    public List<Report> ModifiedReports { get; set; } = null!;
}

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableNameConstants.Users).OwnsOne(x => x.Settings).WithOwner();
        builder
            .HasMany(x => x.CreatedReports)
            .WithOne(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasMany(x => x.ModifiedReports)
            .WithOne(x => x.LastModifiedBy)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
