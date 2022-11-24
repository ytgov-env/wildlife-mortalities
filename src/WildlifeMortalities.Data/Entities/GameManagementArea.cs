using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities;

public class GameManagementArea
{
    public int Id { get; set; }
    public string Zone { get; set; } = string.Empty;
    public string Subzone { get; set; } = string.Empty;
    public string Area { get; } = string.Empty;

    public static AllSpecies ResolveSubType(AllSpecies species) =>
        // Todo implement herd/group resolution logic
        species;
}

public class GameManagementAreaConfig : IEntityTypeConfiguration<GameManagementArea>
{
    public void Configure(EntityTypeBuilder<GameManagementArea> builder) =>
        builder.Property(a => a.Area).HasComputedColumnSql("[Zone] + '-' + [Subzone]", true);
}
