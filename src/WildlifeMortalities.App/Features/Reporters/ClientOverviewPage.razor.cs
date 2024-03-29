﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.App.Extensions;
using WildlifeMortalities.App.Features.Shared;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.App.Features.Reporters;

public partial class ClientOverviewPage : DbContextAwareComponent, IDisposable
{
    private EditContext _editContext = default!;

    private SelectClientViewModel _selectedClientViewModel = default!;

    private int? _draftReportCount;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Parameter]
    public EventCallback<bool> ValidationChanged { get; set; }

    [Parameter]
    public string? EnvClientId { get; set; }

    private readonly DraftCounter _draftCounter = new();

    private void CounterChanged(object? _, EventArgs __)
    {
        InvokeAsync(StateHasChanged);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (EnvClientId != null)
        {
            using var context = GetContext();

            _selectedClientViewModel.SelectedClient ??= await context.People
                .OfType<Client>()
                .Where(c => c.EnvPersonId.StartsWith(EnvClientId))
                .FirstOrDefaultAsync();
        }

        await base.OnParametersSetAsync();
    }

    protected override void OnInitialized()
    {
        _selectedClientViewModel = new SelectClientViewModel();
        _editContext = new EditContext(_selectedClientViewModel);
        _editContext.OnFieldChanged += Context_OnFieldChanged;
        _draftCounter.CountChanged += CounterChanged;

        base.OnInitialized();
    }

    private void ClientSelected(Client? client)
    {
        _selectedClientViewModel.SelectedClient = client;
        EnvClientId = _selectedClientViewModel.SelectedClient.EnvPersonId;
        NavigationManager.NavigateTo(Constants.Routes.GetClientOverviewPageLink(EnvClientId));
    }

    private void Context_OnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        _editContext.Validate();
        ValidationChanged.InvokeAsync(!_editContext.GetValidationMessages().Any());
        EnvClientId = _selectedClientViewModel.SelectedClient.EnvPersonId;
    }

    private async Task<IEnumerable<Client>> SearchClientByEnvClientIdOrLastName(string searchTerm)
    {
        using var context = GetContext();

        return await context.People
            .OfType<Client>()
            .SearchByEnvClientIdOrLastName(searchTerm)
            .ToArrayAsync();
    }

    public void Dispose()
    {
        if (_editContext is not null)
        {
            _editContext.OnFieldChanged -= Context_OnFieldChanged;
        }

        if (_draftCounter != null)
        {
            _draftCounter.CountChanged -= CounterChanged;
        }
    }

    public async Task HandleDraftReportCount(int count) =>
        await Task.FromResult(_draftReportCount = count);
}
