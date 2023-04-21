using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using WildlifeMortalities.App.Extensions;
using WildlifeMortalities.App.Features.Shared;
using WildlifeMortalities.Data.Entities.People;

namespace WildlifeMortalities.App.Features.Reporters;

public partial class ClientOverviewPage : DbContextAwareComponent
{
    private EditContext _editContext = default!;

    private SelectClientViewModel _selectedClientViewModel = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Parameter]
    public EventCallback<bool> ValidationChanged { get; set; }

    [Parameter]
    public string? EnvClientId { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        if (EnvClientId != null)
        {
            _selectedClientViewModel.SelectedClient ??= await Context.People
                .OfType<Client>()
                .Where(c => c.EnvClientId.StartsWith(EnvClientId))
                .FirstOrDefaultAsync();
        }

        await base.OnParametersSetAsync();
    }

    protected override void OnInitialized()
    {
        _selectedClientViewModel = new SelectClientViewModel();
        _editContext = new EditContext(_selectedClientViewModel);
        _editContext.OnFieldChanged += Context_OnFieldChanged;

        base.OnInitialized();
    }

    private void ClientSelected(Client? client)
    {
        _selectedClientViewModel.SelectedClient = client;
        EnvClientId = _selectedClientViewModel.SelectedClient.EnvClientId;
        NavigationManager.NavigateTo(Constants.Routes.GetClientOverviewPageLink(EnvClientId));
    }

    private void Context_OnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        _editContext.Validate();
        ValidationChanged.InvokeAsync(!_editContext.GetValidationMessages().Any());
        EnvClientId = _selectedClientViewModel.SelectedClient.EnvClientId;
    }

    private async Task<IEnumerable<Client>> SearchClientByEnvClientIdOrLastName(
        string searchTerm
    ) =>
        await Context.People
            .OfType<Client>()
            .SearchByEnvClientIdOrLastName(searchTerm)
            .ToArrayAsync();

    protected override void Dispose(bool disposing)
    {
        if (_editContext is not null)
        {
            _editContext.OnFieldChanged -= Context_OnFieldChanged;
        }
        base.Dispose(disposing);
    }
}
