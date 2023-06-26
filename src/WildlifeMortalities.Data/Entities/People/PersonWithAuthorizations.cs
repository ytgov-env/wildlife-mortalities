using WildlifeMortalities.Data.Entities.Authorizations;
using WildlifeMortalities.Data.Entities.Rules.BagLimit;

namespace WildlifeMortalities.Data.Entities.People;

public abstract class PersonWithAuthorizations : Person
{
    public string EnvPersonId { get; set; } = string.Empty;
    public List<Authorization> Authorizations { get; set; } = null!;

    public List<BagEntry> BagEntries { get; set; } = null!;

    public abstract void Update(PersonWithAuthorizations person);

    public abstract bool Merge(PersonWithAuthorizations person);
}
