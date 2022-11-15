using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class Authorization
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTime ValidFromDate { get; set; }
    public DateTime ValidToDate { get; set; }
    public string Season => $"{ValidFromDate.Year}-{ValidToDate.Year}";
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
}

public class AuthorizationConfig : IEntityTypeConfiguration<Authorization>
{
    public void Configure(EntityTypeBuilder<Authorization> builder)
    {
    }
}
