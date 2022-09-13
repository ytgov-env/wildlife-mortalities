using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WildlifeMortalities.Shared.Models;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Reporters;

public partial class ClientOverviewPage : IDisposable
{
    private EditContext _context = null!;

    protected override void OnInitialized()
    {
        _selectedClientViewModel = new();
        _context = new EditContext(_selectedClientViewModel);
        _context.OnFieldChanged += _context_OnFieldChanged;
    }

    private void _context_OnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        _context.Validate();
        ValidationChanged.InvokeAsync(!_context.GetValidationMessages().Any());
    }

    [Inject]
    private IClientLookupService ClientLookupService { get; set; } = null!;

    private SelectClientViewModel _selectedClientViewModel = null!;

    private async Task<IEnumerable<ClientDto>> SearchClientByEnvClientIdOrLastName(string input) =>
        (await ClientLookupService.SearchByEnvClientId(input))
            .Union(await ClientLookupService.SearchByLastName(input))
            .OrderBy(x => x.LastName);

    public void Dispose()
    {
        if (_context is not null)
        {
            _context.OnFieldChanged -= _context_OnFieldChanged;
        }
    }

    [Parameter]
    public EventCallback<bool> ValidationChanged { get; set; }

    [Parameter]
    public int Id { get; set; }
}
