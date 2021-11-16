using Niravadea.Projects.Orchid.TokenTracker;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.TokenTracker.Services
{
    public interface ITokenTracker
    {
        Task<string> AddPendingAuthentication(ulong discordId, int forumsId);

        Task<IdTokenPair> GetPendingAuthenticationToken(ulong discordId);

        Task ClearPendingAuthenticationToken(ulong discordId);
    }
}
