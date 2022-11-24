using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WildlifeMortalities.Data.Entities.People;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.Reporters;

public partial class ConservationOfficerOverviewPage : IDisposable
{
    private EditContext _context = null!;

    private SelectConservationOfficerViewModel _selectedConservationOfficerViewModel = null!;

    [Parameter] public EventCallback<bool> ValidationChanged { get; set; }

    [Parameter] public int Id { get; set; }

    [Inject] private ConservationOfficerService ConservationOfficerService { get; set; } = default!;

    public void Dispose()
    {
        if (_context is not null)
        {
            _context.OnFieldChanged -= _context_OnFieldChanged;
        }
    }

    protected override void OnInitialized()
    {
        _selectedConservationOfficerViewModel = new SelectConservationOfficerViewModel();
        _context = new EditContext(_selectedConservationOfficerViewModel);
        _context.OnFieldChanged += _context_OnFieldChanged;
    }

    private void _context_OnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        _context.Validate();
        ValidationChanged.InvokeAsync(!_context.GetValidationMessages().Any());
    }

    private async Task<
        IEnumerable<ConservationOfficer>
    > SearchConservationOfficerByBadgeNumberOrLastName(string input) =>
        (await ConservationOfficerService.SearchByBadgeNumber(input))
        .Union(await ConservationOfficerService.SearchByLastName(input))
        .OrderBy(x => x.LastName);
}
