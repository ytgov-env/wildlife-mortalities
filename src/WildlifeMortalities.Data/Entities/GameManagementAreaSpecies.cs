using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities;

public class GameManagementAreaSpecies
{
    public int Id { get; set; }
    public HuntedSpeciesWithGameManagementArea Species { get; set; }
    public int GameManagementAreaId { get; set; }
    public GameManagementArea GameManagementArea { get; set; } = null!;
    public List<GameManagementAreaSchedule> Schedules { get; set; } = null!;
    public List<GameManagementUnit?> GameManagementUnits { get; set; } = null!;
}

public class GameManagementAreaSpeciesConfig : IEntityTypeConfiguration<GameManagementAreaSpecies>
{
    public void Configure(EntityTypeBuilder<GameManagementAreaSpecies> builder)
    {
        builder
            .Property(s => s.Species)
            .HasConversion<string>()
            .HasMaxLength(25);
    }
}
