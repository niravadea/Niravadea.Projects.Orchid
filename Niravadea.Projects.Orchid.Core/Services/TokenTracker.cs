using MediatR;
using Niravadea.Projects.Orchid.Core;
using Niravadea.Projects.Orchid.Core.Requests.Tokens;
using Niravadea.Projects.Orchid.Core.Requests;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Core.Services
{
    public class TokenTracker : ITokenTracker
    {
        private readonly IDictionary<ulong, IdTokenPair> _pendingAuthentications = new Dictionary<ulong, IdTokenPair>();
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly IMediator _mediator;

        public TokenTracker(IMediator mediator) => _mediator = mediator;

        public async Task<string> AddPendingAuthentication(ulong discordId, int forumsId)
        {
            try
            {
                await _semaphoreSlim.WaitAsync();

                if (_pendingAuthentications.ContainsKey(discordId))
                {
                    return _pendingAuthentications[discordId].ExpectedToken;
                }
                else
                {
                    string newToken = await _mediator.Send(TokenGenerationRequest.NewRequest());
                    _pendingAuthentications.Add(discordId, new IdTokenPair { ForumsId = forumsId, ExpectedToken = newToken });
                    return newToken;
                }
            }
            catch
            {
                // idk, I'll ponder this when I'm sober
                throw;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task<IdTokenPair> GetPendingAuthenticationToken(ulong discordId)
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                return _pendingAuthentications.ContainsKey(discordId) ? _pendingAuthentications[discordId] : null;
            }
            catch
            {
                throw;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task ClearPendingAuthenticationToken(ulong discordId)
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                _pendingAuthentications.Remove(key: discordId);
            }
            catch
            {
                throw;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }
}
