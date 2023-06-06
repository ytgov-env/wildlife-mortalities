using WildlifeMortalities.Data.Entities.People;
using static WildlifeMortalities.Data.Rules.BagLimitRule;

namespace WildlifeMortalities.Data.Entities.Rules.BagLimit;

public class BagLimitEntryPerPerson
{
    public int Id { get; set; }
    public int CurrentValue { get; private set; }
    public int SharedValue { get; private set; }
    public int Total => CurrentValue + SharedValue;

    public PersonWithAuthorizations Person { get; init; } = null!;
    public BagLimitEntry BagLimitEntry { get; init; } = null!;

    internal void Increase(
        AppDbContext context,
        ICollection<BagLimitEntryPerPerson> personalEntries
    )
    {
        CurrentValue++;

        foreach (var shared in BagLimitEntry.SharedWith)
        {
            var existingSharedEntry = personalEntries.FirstOrDefault(
                x => x.BagLimitEntry == shared
            );
            if (existingSharedEntry != null)
                existingSharedEntry.SharedValue++;
            else
            {
                var entry = new BagLimitEntryPerPerson
                {
                    BagLimitEntry = shared,
                    Person = Person,
                    SharedValue = 1
                };

                personalEntries.Add(entry);
                context.BagLimitEntriesPerPerson.Add(entry);
            }
        }
    }
}
