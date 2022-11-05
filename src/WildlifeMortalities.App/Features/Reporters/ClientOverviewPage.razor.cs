using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Shared.Services.ClientLookup;

namespace WildlifeMortalities.App.Features.Reporters;

public partial class ClientOverviewPage : IDisposable
{
    private EditContext _context = default!;

    private SelectClientViewModel _selectedClientViewModel = default!;

    [Inject] private IClientLookupService ClientLookupService { get; set; } = default!;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public EventCallback<bool> ValidationChanged { get; set; }

    [Parameter] public int Id { get; set; }

    public void Dispose()
    {
        if (_context is not null)
        {
            _context.OnFieldChanged -= _context_OnFieldChanged;
        }
    }

    protected override void OnInitialized()
    {
        _selectedClientViewModel = new SelectClientViewModel();
        _context = new EditContext(_selectedClientViewModel);
        _context.OnFieldChanged += _context_OnFieldChanged;
    }

    private void _context_OnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        _context.Validate();
        ValidationChanged.InvokeAsync(!_context.GetValidationMessages().Any());
        // TODO Is there a better way to set the route parameter?
        Id = _selectedClientViewModel.SelectedClient.Id;
        NavigationManager.NavigateTo($"/reporters/clients/{Id}");
    }

    private async Task<IEnumerable<Client>> SearchClientByEnvClientIdOrLastName(string input) =>
        (await ClientLookupService.SearchByEnvClientId(input))
        .Union(await ClientLookupService.SearchByLastName(input))
        .OrderBy(x => x.LastName);
}
