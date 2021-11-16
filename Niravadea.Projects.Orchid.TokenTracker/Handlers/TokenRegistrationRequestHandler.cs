using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Niravadea.Projects.Orchid.TokenTracker.Services;
using Niravadea.Projects.Orchid.TokenTracker.Requests;
using Niravadea.Projects.Orchid.Shared.Requests;

namespace Niravadea.Projects.Orchid.TokenTracker.Handlers
{
    public class TokenRegistrationRequestHandler : IRequestHandler<TokenRegistrationRequest>
    {
        private readonly ITokenTracker _authenticationTracker;
        private readonly IMediator _mediator;

        public TokenRegistrationRequestHandler(IMediator mediator, ITokenTracker authenticationTracker)
        {
            _mediator = mediator;
            _authenticationTracker = authenticationTracker;
        }

        public async Task<Unit> Handle(TokenRegistrationRequest request, CancellationToken cancellationToken)
        {
            // access the database, make a token for the user, send the user the token
            string token = await _authenticationTracker.AddPendingAuthentication(
                discordId: request.UserId,
                forumsId: request.ForumsId
            );

            await _mediator.Send(CompleteSuccessfulInteractionRequest.NewRequestFromMessage(
                interactionId: request.InteractionId,
                message: $"Your token is `{token}`.  Add this to any field in your profile and then use the `/Validate` command."
            ));
            return Unit.Value;
        }
    }
}
