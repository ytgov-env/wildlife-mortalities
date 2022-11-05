using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Authorizations;

namespace WildlifeMortalities.Data.Entities.People;

public class Client : Person
{
    public string EnvClientId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public List<Authorization> Authorizations { get; set; } = null!;
}

public class ClientConfig : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder) => builder.HasIndex(c => c.EnvClientId).IsUnique();
}
