using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WildlifeMortalities.Shared.Models;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Reporters;

public partial class ClientOverviewPage : IDisposable
{
    private EditContext _context = null!;

    private SelectClientViewModel _selectedClientViewModel = null!;

    [Inject] private IClientLookupService ClientLookupService { get; set; } = null!;

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
    }

    private async Task<IEnumerable<ClientDto>> SearchClientByEnvClientIdOrLastName(string input) =>
        (await ClientLookupService.SearchByEnvClientId(input))
        .Union(await ClientLookupService.SearchByLastName(input))
        .OrderBy(x => x.LastName);
}
