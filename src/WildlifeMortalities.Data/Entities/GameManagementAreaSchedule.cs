using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Enums;

namespace WildlifeMortalities.Data.Entities;

public class GameManagementAreaSchedule
{
    public int Id { get; set; }
    public int GameManagementAreaSpeciesId { get; set; }
    public GameManagementAreaSpecies GameManagementAreaSpecies { get; set; } = null!;
    public GameManagementAreaStatus Status { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}

public class GameManagementAreaScheduleConfig : IEntityTypeConfiguration<GameManagementAreaSchedule>
{
    public void Configure(EntityTypeBuilder<GameManagementAreaSchedule> builder)
    {
        builder.Property(s => s.PeriodStart).HasColumnType("date");
        builder.Property(s => s.PeriodEnd).HasColumnType("date");
    }
}
