using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildlifeMortalities.Shared.Models;

namespace WildlifeMortalities.Shared.Services;

public interface IClientLookupService
{
    Task<IEnumerable<ClientDto>> SearchByLastName(string input);
    Task<IEnumerable<ClientDto>> SearchByEnvClientId(string input);
}
