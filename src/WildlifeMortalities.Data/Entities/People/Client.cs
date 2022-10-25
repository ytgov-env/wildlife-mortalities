using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.Authorizations;

namespace WildlifeMortalities.Data.Entities.People;

public class Client : Person
{
    public string EnvClientId { get; set; } = string.Empty;
    public List<Authorization> Authorizations { get; set; } = null!;
}

public class ClientConfig : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder) => builder.HasIndex(c => c.EnvClientId).IsUnique();
}
