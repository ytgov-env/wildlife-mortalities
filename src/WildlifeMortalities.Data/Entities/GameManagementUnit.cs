using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities;

public class GameManagementUnit
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<GameManagementAreaSpecies> GameManagementAreaSpecies { get; set; } = null!;
    public DateTime ActiveFrom { get; set; }
    public DateTime ActiveTo { get; set; }
}

class GameManagementUnitConfig : IEntityTypeConfiguration<GameManagementUnit>
{
    public void Configure(EntityTypeBuilder<GameManagementUnit> builder)
    {
        builder.Property(u => u.ActiveFrom).HasColumnType("date");
        builder.Property(u => u.ActiveTo).HasColumnType("date");
        builder.Property(u => u.Name).HasMaxLength(50);
    }
}
