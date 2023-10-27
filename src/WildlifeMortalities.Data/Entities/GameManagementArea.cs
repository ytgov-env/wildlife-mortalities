using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;

namespace WildlifeMortalities.Data.Entities;

public class GameManagementArea
{
    public int Id { get; set; }
    public string Zone { get; set; } = string.Empty;
    public string Subzone { get; set; } = string.Empty;
    public string Area { get; } = string.Empty;

    public List<GameManagementSubArea> SubAreas { get; set; } = null!;

    public List<HuntingBagLimitEntry> HuntingBagLimitEntries { get; set; } = null!;

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

public class GameManagementSubArea
{
    public int Id { get; set; }
    public string SubArea { get; set; } = string.Empty;
    public int GameManagementAreaId { get; set; }
    public GameManagementArea GameManagementArea { get; set; } = null!;
}

public class GameManagementAreaSubConfig : IEntityTypeConfiguration<GameManagementSubArea>
{
    public void Configure(EntityTypeBuilder<GameManagementSubArea> builder)
    {
        builder.Property(g => g.SubArea).HasMaxLength(2);
    }
}
