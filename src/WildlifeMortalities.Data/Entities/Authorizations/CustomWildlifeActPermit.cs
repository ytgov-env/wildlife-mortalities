using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class CustomWildlifeActPermit : Authorization
{
    [Column($"{nameof(CustomWildlifeActPermit)}_{nameof(Conditions)}")]
    public string Conditions { get; set; } = string.Empty;

    public override string GetAuthorizationType() => "Custom wildlife act permit";

    protected override void UpdateInternal(Authorization authorization)
    {
        if (authorization is not CustomWildlifeActPermit customWildlifeActPermit)
        {
            throw new ArgumentException(
                $"Expected {nameof(CustomWildlifeActPermit)} but received {authorization.GetType().Name}"
            );
        }

        Conditions = customWildlifeActPermit.Conditions;
    }
}

public class CustomWildlifeActPermitConfig : IEntityTypeConfiguration<CustomWildlifeActPermit>
{
    public void Configure(EntityTypeBuilder<CustomWildlifeActPermit> builder) =>
        builder.ToTable(TableNameConstants.Authorizations);
}
