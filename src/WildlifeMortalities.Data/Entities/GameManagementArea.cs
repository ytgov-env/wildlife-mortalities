using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WildlifeMortalities.Data.Entities;

public class GameManagementArea
{
    public int Id { get; set; }
    public string Zone { get; set; } = string.Empty;
    public string Subzone { get; set; } = string.Empty;
    public string Area { get; } = string.Empty;

    public static Species ResolveSubType(Species species) =>
        // Todo implement herd/group resolution logic
        species;

    public override string ToString() =>
        string.IsNullOrWhiteSpace(Area) ? $"{Zone}-{Subzone}" : Area;
}

public class GameManagementAreaConfig : IEntityTypeConfiguration<GameManagementArea>
{
    public void Configure(EntityTypeBuilder<GameManagementArea> builder)
    {
        builder.Property(g => g.Zone).HasMaxLength(3);
        builder.Property(g => g.Subzone).HasMaxLength(3);
        builder.Property(a => a.Area).HasComputedColumnSql("[Zone] + '-' + [Subzone]", true);
    }
}
