using Microsoft.AspNetCore.Components;
using WildlifeMortalities.Data.Entities.Reporters;
using WildlifeMortalities.Data.Enums;
using WildlifeMortalities.Shared.Models;
using WildlifeMortalities.Shared.Services;

namespace WildlifeMortalities.App.Features.HarvestReports;

public partial class CreateHarvestReportPage
{
    [Inject]
    private IClientLookupService ClientLookupService { get; set; }

    private enum ReporterType
    {
        Client,
        ConservationOfficer
    }

    private ReporterType _reporterType;
    private ClientDto _selectedClient;
    private ConservationOfficer _selectedConservationOfficer;

    private async Task<IEnumerable<ClientDto>> SearchClientByEnvClientIdOrLastName(string input) =>
        (await ClientLookupService.SearchByEnvClientId(input))
            .Union(await ClientLookupService.SearchByLastName(input))
            .OrderBy(x => x.LastName);

    private bool IsClientSelected() => _selectedClient is null;

    private async Task<IEnumerable<ConservationOfficer>> SearchConservationOfficerByBadgeNumber(
        string input
    ) => throw new NotImplementedException();
}
