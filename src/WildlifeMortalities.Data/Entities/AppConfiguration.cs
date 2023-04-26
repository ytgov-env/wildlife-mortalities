using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WildlifeMortalities.Data.Entities;

public class AppConfiguration
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class AppConfigurationConfig : IEntityTypeConfiguration<AppConfiguration>
{
    public void Configure(EntityTypeBuilder<AppConfiguration> builder)
    {
        builder.HasKey(x => x.Key);
    }
}
