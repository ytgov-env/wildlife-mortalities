using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;
using WildlifeMortalities.Data.Extensions;

namespace WildlifeMortalities.Data.Entities.Rules.BagLimit;

public class BagEntry
{
    public int Id { get; set; }
    public int CurrentValue { get; private set; }
    public int SharedValue { get; private set; }
    public int TotalValue => CurrentValue + SharedValue;
    public PersonWithAuthorizations Person { get; init; } = null!;
    public BagLimitEntry BagLimitEntry { get; init; } = null!;

    internal string[] GetSpeciesDescriptions()
    {
        var species = new List<Species> { BagLimitEntry.Species };

        species.AddRange(BagLimitEntry.SharedWithDifferentSpeciesAndOrSex.Select(x => x.Species));

        return species.Select(x => x.GetDisplayName().ToLower()).ToArray();
    }

    internal bool Increase(
        AppDbContext context,
        HarvestActivity activity,
        ICollection<BagEntry> personalEntries
    )
    {
        var hasExceeded = false;
        CurrentValue++;
        if (TotalValue > BagLimitEntry.MaxValuePerPerson)
        {
            hasExceeded = true;
        }

        BagLimitEntry.AddToQueue(activity);

        foreach (var shared in BagLimitEntry.SharedWithDifferentSpeciesAndOrSex)
        {
            var sharedPersonalEntry = personalEntries.FirstOrDefault(
                x => x.BagLimitEntry == shared
            );
            if (sharedPersonalEntry == null)
            {
                sharedPersonalEntry = new BagEntry { BagLimitEntry = shared, Person = Person, };

                personalEntries.Add(sharedPersonalEntry);
                context.BagEntries.Add(sharedPersonalEntry);
            }

            sharedPersonalEntry.SharedValue++;
            if (
                sharedPersonalEntry.TotalValue > sharedPersonalEntry.BagLimitEntry.MaxValuePerPerson
            )
            {
                hasExceeded = true;
            }
        }

        return hasExceeded;
    }
}
