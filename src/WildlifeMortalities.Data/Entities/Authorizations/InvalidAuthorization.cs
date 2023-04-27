using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class InvalidAuthorization
{
    public InvalidAuthorization() { }

    public InvalidAuthorization(Authorization authorization)
    {
        Number = authorization.Number;
        Type = authorization.GetAuthorizationType();
        ValidFromDateTime = authorization.ValidFromDateTime;
        ValidToDateTime = authorization.ValidToDateTime;
        DateCreated = DateTimeOffset.Now;
        Season = authorization.Season;
        Client = authorization.Client;
    }

    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTimeOffset ValidFromDateTime { get; set; }
    public DateTimeOffset ValidToDateTime { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public int? SeasonId { get; set; }
    public Season? Season { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public string GetIdentifier() => GetIdentifier(this);

    public static string GetIdentifier(InvalidAuthorization input) =>
        $"{input.Type}-{input.Number}-{input.ClientId}";
}
