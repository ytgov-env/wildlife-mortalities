using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Data.Entities.Reports.SingleMortality;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public abstract class Authorization
{
    public int Id { get; set; }
    public string? PosseId { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTimeOffset ValidFromDateTime { get; set; }
    public DateTimeOffset ValidToDateTime { get; set; }
    public DateTimeOffset LastModifiedDateTime { get; set; }
    public int SeasonId { get; set; }
    public Season Season { get; set; } = null!;

    public int PersonId { get; set; }
    public PersonWithAuthorizations Person { get; set; } = null!;
    public bool IsCancelled { get; set; }
    public List<HarvestActivity> Activities { get; set; } = null!;

    public abstract string GetAuthorizationType();

    protected abstract void UpdateInternal(Authorization authorization);

    public void Update(Authorization authorization)
    {
        Number = authorization.Number;
        IsCancelled = Number.EndsWith('C');
        ValidFromDateTime = authorization.ValidFromDateTime;
        ValidToDateTime = authorization.ValidToDateTime;
        LastModifiedDateTime = authorization.LastModifiedDateTime;
        Season = authorization.Season;

        UpdateInternal(authorization);
    }

    private string GetNormalizedNumber() => Number.EndsWith('C') ? Number[..^1] : Number;

    public string GetUniqueIdentifier()
    {
        return PosseId
            ?? $"{GetNormalizedNumber()}-{GetAuthorizationType()}-{Person.Id}-{Season.Id}";
    }

    public string GetUniqueIdentifierWithoutSeason()
    {
        return PosseId ?? $"{GetNormalizedNumber()}-{GetAuthorizationType()}-{Person.Id}-{0}";
    }
}

public class AuthorizationConfig : IEntityTypeConfiguration<Authorization>
{
    public void Configure(EntityTypeBuilder<Authorization> builder) { }
}
