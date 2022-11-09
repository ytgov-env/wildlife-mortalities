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

    [Parameter] public int Id { get; set; }

    public void Dispose()
    {
        if (_editContext is not null)
        {
            _editContext.OnFieldChanged -= EditContextOnFieldChanged;
        }
    }

    protected override void OnInitialized()
    {
        _selectedClientViewModel = new SelectClientViewModel();
        _editContext = new EditContext(_selectedClientViewModel);
        _editContext.OnFieldChanged += EditContextOnFieldChanged;
    }

    private void EditContextOnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        _editContext.Validate();
        ValidationChanged.InvokeAsync(!_editContext.GetValidationMessages().Any());
        // TODO Is there a better way to set the route parameter?
        Id = _selectedClientViewModel.SelectedClient.Id;
        NavigationManager.NavigateTo($"/reporters/clients/{Id}");
    }

    private async Task<IEnumerable<Client>> SearchClientByEnvClientIdOrLastName(string input)
    {
        return (await ClientService.SearchByEnvClientId(input))
            .Union(await ClientService.SearchByLastName(input))
            .OrderBy(x => x.LastName);
    }
}
