using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities;

public class OutfitterArea
{
    public int Id { get; set; }
    public string Area { get; set; }
}

public class OutfitterAreaConfig : IEntityTypeConfiguration<OutfitterArea>
{
    public void Configure(EntityTypeBuilder<OutfitterArea> builder) => builder.HasIndex(a => a.Area).IsUnique();
}
