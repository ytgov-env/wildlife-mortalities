// ReSharper disable InconsistentNaming

using static WildlifeMortalities.Shared.Services.Posse.PosseService;

namespace WildlifeMortalities.Shared.Services.Posse;

public class GetClientsResponse
{
    public IEnumerable<ClientDto> Clients { get; set; } = null!;
}
