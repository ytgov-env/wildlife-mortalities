using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Extensions;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Rules.BagLimit;

public class BagEntry
{
    public int Id { get; set; }
    public int CurrentValue { get; private set; }
    public int SharedValue { get; private set; }
    public int TotalValue => CurrentValue + SharedValue;
    public int PersonId { get; init; }
    public PersonWithAuthorizations Person { get; init; } = null!;
    public int BagLimitEntryId { get; init; }
    public BagLimitEntry BagLimitEntry { get; init; } = null!;

    public string[] GetSpeciesDescriptions()
    {
        var species = new List<Species> { BagLimitEntry.Species };

        species.AddRange(BagLimitEntry.MaxValuePerPersonSharedWith.Select(x => x.Species));

        return species.Select(x => x.GetDisplayName().ToLower()).Distinct().ToArray();
    }

    public bool Increase(
        AppDbContext context,
        HarvestActivity activity,
        ICollection<BagEntry> bagEntries
    )
    {
        var hasExceeded = false;
        CurrentValue++;
        if (TotalValue > BagLimitEntry.MaxValuePerPerson)
        {
            hasExceeded = true;
        }

        BagLimitEntry.AddToQueue(activity);

        foreach (var sharedBagLimitEntries in BagLimitEntry.MaxValuePerPersonSharedWith)
        {
            var sharedBagEntry = bagEntries.FirstOrDefault(
                x => x.BagLimitEntry == sharedBagLimitEntries
            );
            if (sharedBagEntry == null)
            {
                sharedBagEntry = new BagEntry
                {
                    BagLimitEntry = sharedBagLimitEntries,
                    Person = Person,
                };

                bagEntries.Add(sharedBagEntry);
                context.BagEntries.Add(sharedBagEntry);
            }

            if (sharedBagEntry.BagLimitEntry.ShouldMutateBagValue(activity))
            {
                sharedBagEntry.SharedValue++;
            }

            if (sharedBagEntry.TotalValue > sharedBagEntry.BagLimitEntry.MaxValuePerPerson)
            {
                hasExceeded = true;
            }
        }

        return hasExceeded;
    }

    public void Decrease(HarvestActivity activity, ICollection<BagEntry> bagEntries)
    {
        CurrentValue--;
        BagLimitEntry.AddToQueue(activity);

        foreach (var sharedBagLimitEntries in BagLimitEntry.MaxValuePerPersonSharedWith)
        {
            var sharedBagEntry = bagEntries.FirstOrDefault(
                x => x.BagLimitEntry == sharedBagLimitEntries
            );

            if (sharedBagEntry == null)
            {
                continue;
            }

            if (sharedBagEntry.BagLimitEntry.ShouldMutateBagValue(activity))
            {
                sharedBagEntry.SharedValue--;
            }
        }
    }
}

public class BagEntryConfig : IEntityTypeConfiguration<BagEntry>
{
    public void Configure(EntityTypeBuilder<BagEntry> builder)
    {
        builder.ToTable(TableNameConstants.BagEntries);
        builder.HasIndex(x => new { x.PersonId, x.BagLimitEntryId }).IsUnique();
    }
}
