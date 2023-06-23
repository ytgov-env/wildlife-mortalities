using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.Reports;
using WildlifeMortalities.Data.Entities.Reports.MultipleMortalities;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Entities.Seasons;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Rules.BagLimit;

public class TrappingBagLimitEntry : BagLimitEntry
{
    public List<RegisteredTrappingConcession> RegisteredTrappingConcessions { get; set; } = null!;
    public TrappingSeason Season { get; set; } = null!;

    public override Season GetSeason()
    {
        return Season;
    }

    //private Expression<Func<BagLimitEntry, Season>> expression = (x) => ((TrappingBagLimitEntry)x).Season;

    protected override bool IsWithinArea(HarvestActivity activity, Report report)
    {
        return RegisteredTrappingConcessions.Any(
            x => x.Id == ((TrappedMortalitiesReport)report).RegisteredTrappingConcession.Id
        );
    }
}

public class TrappingBagLimitEntryConfig : IEntityTypeConfiguration<TrappingBagLimitEntry>
{
    public void Configure(EntityTypeBuilder<TrappingBagLimitEntry> builder)
    {
        builder.ToTable(TableNameConstants.BagLimitEntries);
        builder
            .HasOne(x => x.Season)
            .WithMany(x => x.BagLimitEntries)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
