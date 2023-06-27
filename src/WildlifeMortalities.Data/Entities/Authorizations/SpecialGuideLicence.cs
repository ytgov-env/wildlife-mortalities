using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports;
using static WildlifeMortalities.Data.Constants;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class SpecialGuideLicence : Authorization
{
    [Column($"{nameof(SpecialGuideLicence)}_{nameof(GuidedClientId)}")]
    public int GuidedClientId { get; set; }
    public Client GuidedClient { get; set; } = null!;

    public override string GetAuthorizationType() => "Special guide licence";

    protected override void UpdateInternal(Authorization authorization)
    {
        if (authorization is not SpecialGuideLicence specialGuideLicence)
        {
            throw new ArgumentException(
                $"Expected {nameof(SpecialGuideLicence)} but received {authorization.GetType().Name}"
            );
        }

        GuidedClient = specialGuideLicence.GuidedClient;
    }
}

public class SpecialGuideLicenceConfig : IEntityTypeConfiguration<SpecialGuideLicence>
{
    public void Configure(EntityTypeBuilder<SpecialGuideLicence> builder)
    {
        builder.ToTable(TableNameConstants.Authorizations);
        builder
            .HasOne(s => s.GuidedClient)
            .WithMany(c => c.SpecialGuideLicencesAsClient)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
