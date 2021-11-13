using Niravadea.Projects.Orchid.Core;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Core.Services
{
    public interface ITokenTracker
    {
        Task<string> AddPendingAuthentication(ulong discordId, int forumsId);

        Task<IdTokenPair> GetPendingAuthenticationToken(ulong discordId);

        Task ClearPendingAuthenticationToken(ulong discordId);
    }
}
