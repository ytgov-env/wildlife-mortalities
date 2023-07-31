using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.App.Extensions;
using WildlifeMortalities.App.Features.Shared;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.App.Features.Reporters;

public partial class ConservationOfficerOverviewPage : DbContextAwareComponent, IDisposable
{
    private EditContext _context = default!;

    private SelectConservationOfficerViewModel _selectedConservationOfficerViewModel = null!;

    [Parameter]
    public EventCallback<bool> ValidationChanged { get; set; }

    [Parameter]
    public string? BadgeNumber { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        if (BadgeNumber != null)
        {
            using var context = GetContext();

            _selectedConservationOfficerViewModel.SelectedConservationOfficer ??=
                await context.People
                    .OfType<ConservationOfficer>()
                    .Where(c => c.BadgeNumber.StartsWith(BadgeNumber))
                    .FirstOrDefaultAsync();
        }
        await base.OnParametersSetAsync();
    }

    protected override void OnInitialized()
    {
        _selectedConservationOfficerViewModel = new SelectConservationOfficerViewModel();
        _context = new EditContext(_selectedConservationOfficerViewModel);
        _context.OnFieldChanged += Context_OnFieldChanged;

        base.OnInitialized();
    }

    private void ConservationOfficerSelected(ConservationOfficer? conservationOfficer)
    {
        _selectedConservationOfficerViewModel.SelectedConservationOfficer = conservationOfficer;
        BadgeNumber = _selectedConservationOfficerViewModel.SelectedConservationOfficer.BadgeNumber;
        NavigationManager.NavigateTo(
            Constants.Routes.GetConservationOfficerOverviewPageLink(BadgeNumber)
        );
    }

    private void Context_OnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        _context.Validate();
        ValidationChanged.InvokeAsync(!_context.GetValidationMessages().Any());
    }

    private async Task<
        IEnumerable<ConservationOfficer>
    > SearchConservationOfficerByBadgeNumberOrLastName(string searchTerm)
    {
        using var context = GetContext();

        return await context.People
            .OfType<ConservationOfficer>()
            .SearchByBadgeNumberOrLastName(searchTerm)
            .ToArrayAsync();
    }

    public void Dispose()
    {
        if (_context is not null)
        {
            _context.OnFieldChanged -= Context_OnFieldChanged;
        }
    }
}
