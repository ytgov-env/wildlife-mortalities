using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.App.Features.Shared;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.App.Features.Reporters;

public partial class DraftReportsTableComponent : DbContextAwareComponent
{
    [Parameter]
    public string? EnvClientId { get; set; }

    public IEnumerable<DraftReportDto> DraftReports { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        var client = await Context.People
            .OfType<Client>()
            .FirstOrDefaultAsync(x => x.EnvClientId == EnvClientId);
        if (client == null)
        {
            throw new ArgumentException($"Client {EnvClientId} not found.", nameof(client));
        }
        DraftReports = await Context.DraftReports
            .Where(x => x.PersonId == client.Id)
            .Select(
                x =>
                    new DraftReportDto()
                    {
                        Id = x.Id,
                        PersonId = x.PersonId,
                        Person = x.Person,
                        Type = x.Type,
                        DateLastModified = x.DateLastModified,
                        DateSubmitted = x.DateSubmitted
                    }
            )
            .ToArrayAsync();
    }
}
