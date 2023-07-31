using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Mortalities;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public abstract class Activity
{
    public int Id { get; set; }

    [JsonConverter(typeof(MostConcreteClassJsonConverter<Mortality>))]
    public Mortality Mortality { get; set; } = null!;
    public List<Authorization> Authorizations { get; set; } = null!;
    public List<Violation> Violations { get; set; } = null!;
    public string Comment { get; set; } = string.Empty;
    public DateTimeOffset CreatedTimestamp { get; set; }

    public void PreserveImmutableValues(Activity existingActivity)
    {
        CreatedTimestamp = existingActivity.CreatedTimestamp;
    }
}

public class ActivityConfig : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable(TableNameConstants.Activities);
        builder
            .HasMany(x => x.Violations)
            .WithOne(x => x.Activity)
            .HasForeignKey(x => x.ActivityId);
    }
}
