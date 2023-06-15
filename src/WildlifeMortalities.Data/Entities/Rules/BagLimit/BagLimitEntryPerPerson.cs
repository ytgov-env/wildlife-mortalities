using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Extensions;

namespace WildlifeMortalities.Data.Entities.Rules.BagLimit;

public class BagLimitEntryPerPerson
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

        species.AddRange(BagLimitEntry.SharedWithSpecies.Select(x => x.Species));

        return species.Select(x => x.GetDisplayName().ToLower()).ToArray();
    }

    internal bool Increase(
        AppDbContext context,
        ICollection<BagLimitEntryPerPerson> personalEntries
    )
    {
        var hasExceeded = false;
        CurrentValue++;
        if (TotalValue > BagLimitEntry.MaxValue)
        {
            hasExceeded = true;
        }

        foreach (var shared in BagLimitEntry.SharedWithSpecies)
        {
            var sharedPersonalEntry = personalEntries.FirstOrDefault(
                x => x.BagLimitEntry == shared
            );
            if (sharedPersonalEntry == null)
            {
                sharedPersonalEntry = new BagLimitEntryPerPerson
                {
                    BagLimitEntry = shared,
                    Person = Person,
                };

                personalEntries.Add(sharedPersonalEntry);
                context.BagLimitEntriesPerPerson.Add(sharedPersonalEntry);
            }

            sharedPersonalEntry.SharedValue++;
            if (sharedPersonalEntry.TotalValue > sharedPersonalEntry.BagLimitEntry.MaxValue)
            {
                hasExceeded = true;
            }
        }

        return hasExceeded;
    }
}
