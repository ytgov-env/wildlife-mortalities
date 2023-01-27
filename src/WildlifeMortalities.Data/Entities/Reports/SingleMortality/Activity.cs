using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Mortalities;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public abstract class Activity
{
    public int Id { get; set; }
    public Mortality Mortality { get; set; } = null!;
    public int? AuthorizationId { get; set; }
    public List<Authorization> Authorizations { get; set; } = null!;
    public string Comment { get; set; } = string.Empty;
    public List<Violation> Violations { get; set; } = null!;
}

public class ActivityConfig : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder) => builder.ToTable("Activities");
}
