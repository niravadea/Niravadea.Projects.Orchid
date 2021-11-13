using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Niravadea.Projects.Orchid.Core.Requests.Tokens;
using Niravadea.Projects.Orchid.Core.Services;

namespace Niravadea.Projects.Orchid.Core.Handlers.Tokens
{
    public class TokenClearRequestHandler : IRequestHandler<TokenClearRequest>
    {
        private readonly ITokenTracker _tokenTracker;

        public TokenClearRequestHandler (ITokenTracker tokenTracker)
        {
            _tokenTracker = tokenTracker;
        }

        public async Task<Unit> Handle(TokenClearRequest request, CancellationToken cancellationToken) {
            await _tokenTracker.ClearPendingAuthenticationToken(
                discordId: request.DiscordId
            );
            return Unit.Value;
        }
    }
}
