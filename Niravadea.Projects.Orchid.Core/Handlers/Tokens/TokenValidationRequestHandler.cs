using DSharpPlus.Entities;
using MediatR;
using Niravadea.Projects.Orchid.AuthenticationDatabase.Requests;
using Niravadea.Projects.Orchid.Core;
using Niravadea.Projects.Orchid.Core.Requests.Interactions;
using Niravadea.Projects.Orchid.Core.Requests.Tokens;
using Niravadea.Projects.Orchid.Core.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Core.Handlers.Tokens
{
    public class TokenValidationRequestHandler : IRequestHandler<TokenValidationRequest>
    {
        private readonly ITokenTracker _authenticationTracker;
        private readonly IMediator _mediator;

        public TokenValidationRequestHandler(ITokenTracker authenticationTracker, IMediator mediator)
        {
            _authenticationTracker = authenticationTracker;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(TokenValidationRequest request, CancellationToken cancellationToken)
        {
            // get the token

            // check the corresponding user ID for the designated token
            IdTokenPair pair = await _authenticationTracker.GetPendingAuthenticationToken(request.UserId);

            if (pair == null)
            {
                await _mediator.Send(CompleteUnsuccessfulInteractionRequest.NewRequestFromMessage(
                    interactionId: request.InteractionId,
                    message: "Unable to find an authentication ticket for you.  Try starting a new authentication process with the `/Authenticate` command."
                ));
                return Unit.Value;
            }

            // check
            bool tokenExists = await _mediator.Send(new TokenScrapeRequest() { InteractionId = request.InteractionId, ForumsId = pair.ForumsId, Token = pair.ExpectedToken });

            if (tokenExists)
            {
                bool added = await _mediator.Send(new AuthenticationCommitRequest
                {
                    ForumsId = pair.ForumsId,
                    DiscordId = request.UserId
                });

                if (added)
                {
                    // need to add in a request to add the authenticated role
                    // here.  if that fails, it's... not the end of the world.
                    // the authentication process technically succeeded, but
                    // the final role assignment step failed.  the user is at
                    // least authenticated, and the server admin needs to fix
                    // their shit
                    await _mediator.Send(GrantRoleRequest.CreateNewRequest(
                        interactionId: request.InteractionId,
                        requestedRole: Constants.AuthenticatedRole
                    ));

                    await _mediator.Send(CompleteSuccessfulInteractionRequest.NewRequestFromMessage(
                        interactionId: request.InteractionId,
                        message: "Completed validation."
                    ));
                }
                else
                {
                    await _mediator.Send(CompleteUnsuccessfulInteractionRequest.NewRequestFromMessage(
                        interactionId: request.InteractionId,
                        message: "Unable to complete validation."
                    ));
                }
            }
            else
            {
                await _mediator.Send(CompleteUnsuccessfulInteractionRequest.NewRequestFromMessage(
                    interactionId: request.InteractionId,
                    message: "Couldn't find the expected token in your profile."
                ));
            }

            return Unit.Value;
        }
    }
}
