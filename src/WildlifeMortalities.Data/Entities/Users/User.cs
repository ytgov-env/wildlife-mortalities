using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WildlifeMortalities.Data.Entities.Users;

public class User
{
    public int Id { get; set; }
    public string Sid { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public UserSettings Settings { get; set; } = null!;
}

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder) =>
        builder.ToTable("Users").OwnsOne(x => x.Settings).WithOwner();
}
