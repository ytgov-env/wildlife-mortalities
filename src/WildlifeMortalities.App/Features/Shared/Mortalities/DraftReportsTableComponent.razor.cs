using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using WildlifeMortalities.App.Features.Reports;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.App.Features.Shared.Mortalities;

public partial class DraftReportsTableComponent : DbContextAwareComponent
{
    [Parameter]
    public string? EnvClientId { get; set; }

    public IEnumerable<DraftReportDto> DraftReports { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    protected override async Task OnParametersSetAsync()
    {
        using var context = GetContext();

        if (EnvClientId != null)
        {
            var client = await context.People
                .OfType<Client>()
                .FirstOrDefaultAsync(x => x.EnvPersonId == EnvClientId);
            if (client == null)
            {
                throw new ArgumentException($"Client {EnvClientId} not found.", nameof(client));
            }
            DraftReports = await context.DraftReports
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
                .OrderBy(x => x.DateLastModified)
                .ToArrayAsync();
        }
        else
        {
            DraftReports = await context.DraftReports
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
                .OrderBy(x => x.DateLastModified)
                .ToArrayAsync();
        }
    }

    public void GotoEdit(TableRowClickEventArgs<DraftReportDto> args)
    {
        if (args.Item.Person is not Client)
        {
            throw new InvalidOperationException("Draft report must be for a client.");
        }
        NavigationManager.NavigateTo(
            Constants.Routes.GetEditDraftReportPageLink(
                EnvClientId ?? (args.Item.Person as Client)!.EnvPersonId,
                args.Item.Id
            )
        );
    }
}
