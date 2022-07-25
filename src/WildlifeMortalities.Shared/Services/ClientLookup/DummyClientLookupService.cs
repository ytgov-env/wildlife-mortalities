using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.Shared.Services;

public class DummyClientLookupService : IClientLookupService
{
    private List<ClientDto> _list = new List<ClientDto>
    {
        new ClientDto
        {
            LastName = "Doe",
            FirstName = "John",
            EnvClientId = "11124"
        },
    };

    public Task<IEnumerable<ClientDto>> SearchByEnvClientId(string input) =>
        Task.FromResult(
            _list.Where(
                x =>
                    String.IsNullOrEmpty(input) == true
                        ? true
                        : x.EnvClientId.ToLower().Contains(input)
            )
        );

    public Task<IEnumerable<ClientDto>> SearchByLastName(string input) =>
        Task.FromResult(
            _list.Where(
                x =>
                    String.IsNullOrEmpty(input) == true
                        ? true
                        : x.LastName.ToLower().Contains(input)
            )
        );
}
