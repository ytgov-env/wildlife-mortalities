using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.App.Features.Reporters;

public partial class ConservationOfficerOverviewPage : IDisposable
{
    private EditContext _context = null!;

    private SelectConservationOfficerViewModel _selectedConservationOfficerViewModel = null!;

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
        throw new NotImplementedException();
}
