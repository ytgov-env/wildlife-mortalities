using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WildlifeMortalities.Data.Entities.Reporters;
using WildlifeMortalities.Shared.Models;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.HarvestReports;

public partial class SelectReporterComponent : IDisposable
{
    private EditContext _context = null!;

    protected override void OnInitialized()
    {
        _selectedSelectReporterViewModel = new();
        _context = new EditContext(_selectedSelectReporterViewModel);
        _context.OnFieldChanged += _context_OnFieldChanged;
    }

    private void _context_OnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        _context.Validate();
        ValidationChanged.InvokeAsync(!_context.GetValidationMessages().Any());
    }

    [Inject]
    private IClientLookupService ClientLookupService { get; set; } = null!;

    private SelectReporterViewModel _selectedSelectReporterViewModel = null!;

    private async Task<IEnumerable<ClientDto>> SearchClientByEnvClientIdOrLastName(string input) =>
        (await ClientLookupService.SearchByEnvClientId(input))
            .Union(await ClientLookupService.SearchByLastName(input))
            .OrderBy(x => x.LastName);

    private async Task<IEnumerable<ConservationOfficer>> SearchConservationOfficerByBadgeNumber(
        string input
    ) => throw new NotImplementedException();

    public void Dispose()
    {
        if (_context is not null)
        {
            _context.OnFieldChanged -= _context_OnFieldChanged;
        }
    }

    [Parameter]
    public EventCallback<bool> ValidationChanged { get; set; }
}
