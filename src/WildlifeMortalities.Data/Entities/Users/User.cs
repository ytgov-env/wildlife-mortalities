using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WildlifeMortalities.Data.Entities.Users;

public class User
{
    public int Id { get; set; }
    public string EmailAddress { get; set; }
    public UserSettings Settings { get; set; }
}

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder) =>
        builder.ToTable("Users").OwnsOne(x => x.Settings).WithOwner();
}
