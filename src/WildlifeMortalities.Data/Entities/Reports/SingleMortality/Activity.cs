using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Mortalities;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Reports.SingleMortality;

public abstract class Activity
{
    public int Id { get; set; }
    public string HumanReadableId { get; set; } = string.Empty;

    [JsonConverter(typeof(MostConcreteClassJsonConverter<Mortality>))]
    public Mortality Mortality { get; set; } = null!;
    public List<Violation> Violations { get; set; } = null!;
    public string Comment { get; set; } = string.Empty;
    public string OccurrenceNumber { get; set; } = string.Empty;
    public DateTimeOffset CreatedTimestamp { get; set; }

    public void PreserveImmutableValues(Activity existingActivity)
    {
        CreatedTimestamp = existingActivity.CreatedTimestamp;
        HumanReadableId = existingActivity.HumanReadableId;
    }

    public void GenerateHumanReadableId()
    {
        var rand = new Random();
        const string EligibleChars = "ABCDEFGHJKLMNPQRSTUVWXYZ123456789";
        HumanReadableId = new string(
            Enumerable.Repeat(EligibleChars, 5).Select(s => s[rand.Next(s.Length)]).ToArray()
        );
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
        builder.HasIndex(a => a.HumanReadableId).IsUnique();
    }
}
