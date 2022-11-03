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

    public AllSpecies ResolveSpecies(AllSpecies species)
    {
        if (species != AllSpecies.Caribou)
        {
            return species;
        }

        // TODO implement GMA resolution logic for caribou
        if (Zone == "11")
        {
            return AllSpecies.WoodlandCaribou;
        }
        else
        {
            return AllSpecies.BarrenGroundCaribou;
        }

    }
}

public class GameManagementAreaConfig : IEntityTypeConfiguration<GameManagementArea>
{
    public void Configure(EntityTypeBuilder<GameManagementArea> builder)
    {
        builder.Property(a => a.Area).HasComputedColumnSql("[Zone] + '-' + [Subzone]", true);
    }
}
