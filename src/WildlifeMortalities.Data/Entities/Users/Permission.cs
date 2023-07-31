using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Users;

public class Permission
{
    public int Id { get; set; }
    public string Value { get; set; } = null!;

    public List<User> Users { get; set; } = null!;
}

public class PermissionConfig : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(TableNameConstants.Permissions);
    }
}

public static class PermissionConstants
{
    public const string CreateReport = "CreateReport";
    public const string BioSubmission_ModifyAge = "BioSubmission_ModifyAge";
}
