﻿using Microsoft.AspNetCore.Components;
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
        if (EnvClientId != null)
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
        else
        {
            DraftReports = await Context.DraftReports
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

    public void GotoEdit(TableRowClickEventArgs<DraftReportDto> args)
    {
        if (args.Item.Person is not Client)
        {
            throw new InvalidOperationException("Draft report must be for a client.");
        }
        NavigationManager.NavigateTo(
            Constants.Routes.GetEditDraftReportPageLink(
                EnvClientId ?? (args.Item.Person as Client)!.EnvClientId,
                args.Item.Id
            )
        );
    }
}