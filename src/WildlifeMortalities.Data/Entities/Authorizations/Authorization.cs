using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class Authorization
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Season => $"{StartDate.Year}-{EndDate.Year}";
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
}

public class AuthorizationConfig : IEntityTypeConfiguration<Authorization>
{
    public void Configure(EntityTypeBuilder<Authorization> builder)
    {
        // This shadow property is referenced during ETL to sync authorization data from its source (POSSE)
        builder.Property<int?>("PosseId");
    }
}
