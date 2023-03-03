using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Reporters;

public partial class ClientOverviewPage : IDisposable
{
    private EditContext _editContext = default!;

    private SelectClientViewModel _selectedClientViewModel = default!;

    [Inject] private ClientService ClientService { get; set; } = default!;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public EventCallback<bool> ValidationChanged { get; set; }

    [Parameter] public string EnvClientId { get; set; } = string.Empty;

    public void Dispose()
    {
        if (_editContext is not null)
        {
            _editContext.OnFieldChanged -= EditContextOnFieldChanged;
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        if (_selectedClientViewModel.SelectedClient == null)
        {
            var result = await ClientService.SearchByEnvClientId(EnvClientId);
            _selectedClientViewModel.SelectedClient = result.FirstOrDefault();
        }

        await base.OnParametersSetAsync();
    }

    protected override void OnInitialized()
    {
        _selectedClientViewModel = new SelectClientViewModel();
        _editContext = new EditContext(_selectedClientViewModel);
        _editContext.OnFieldChanged += EditContextOnFieldChanged;
    }

    private void ClientSelected(Client? client)
    {
        _selectedClientViewModel.SelectedClient = client;
        EnvClientId = _selectedClientViewModel.SelectedClient.EnvClientId;
        NavigationManager.NavigateTo($"/reporters/clients/{EnvClientId}");
    }

    private void EditContextOnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        _editContext.Validate();
        ValidationChanged.InvokeAsync(!_editContext.GetValidationMessages().Any());
        EnvClientId = _selectedClientViewModel.SelectedClient.EnvClientId;
    }

    private async Task<IEnumerable<Client>> SearchClientByEnvClientIdOrLastName(string input) =>
        (await ClientService.SearchByEnvClientId(input))
        .Union(await ClientService.SearchByLastName(input))
        .OrderBy(x => x.LastName);
}
