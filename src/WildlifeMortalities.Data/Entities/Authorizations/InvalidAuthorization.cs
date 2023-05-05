using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.Data.Entities.Authorizations;

public class InvalidAuthorization
{
    private InvalidAuthorization() { }

    public InvalidAuthorization(Authorization authorization, string validationErrorMessage)
    {
        PosseId = authorization.PosseId;
        Number = authorization.Number;
        Type = authorization.GetAuthorizationType();
        ValidFromDateTime = authorization.ValidFromDateTime;
        ValidToDateTime = authorization.ValidToDateTime;
        DateCreated = DateTimeOffset.Now;
        Season = authorization.Season;
        Person = authorization.Person;
        ValidationErrorMessage = validationErrorMessage;
    }

    public int Id { get; set; }
    public string? PosseId { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTimeOffset ValidFromDateTime { get; set; }
    public DateTimeOffset ValidToDateTime { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public int? SeasonId { get; set; }
    public Season? Season { get; set; }
    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;
    public string ValidationErrorMessage { get; set; } = string.Empty;

    public string GetUniqueIdentifier() => $"{Type}-{Number}-{Person.Id}-{Season.Id}";
}
