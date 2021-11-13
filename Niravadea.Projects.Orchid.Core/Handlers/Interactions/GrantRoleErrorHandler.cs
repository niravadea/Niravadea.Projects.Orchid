using DSharpPlus.Exceptions;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Niravadea.Projects.Orchid.Core.Exceptions;
using Niravadea.Projects.Orchid.Core.Requests.Interactions;
using System.Threading;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Core.Handlers.Interactions
{
    public class GrantRoleErrorHandler
        : IRequestExceptionAction<GrantRoleRequest, RoleNotFoundException>
        , IRequestExceptionAction<GrantRoleRequest, UnauthorizedException>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GrantRoleErrorHandler> _logger;

        public GrantRoleErrorHandler(
            IMediator mediator,
            ILogger<GrantRoleErrorHandler> logger
        )
        {
            _mediator = mediator;
            _logger = logger;
        }

        // external server side issue
        public async Task Execute(GrantRoleRequest request, RoleNotFoundException exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An error occurred while trying to grant an authenticated role.");
            await _mediator.Send(CompleteUnsuccessfulInteractionRequest.NewRequestFromMessage(
                interactionId: request.InteractionId,
                message: "Completed authentication, but the server does not have the necessary role!  Please `/authenticate` again after the server adds the necessary role."
            ));
        }

        // external server side issue
        public async Task Execute(GrantRoleRequest request, UnauthorizedException exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, $"An error occurred while trying to grant an authenticated role on interaction '{request.InteractionId}'.");
            await _mediator.Send(CompleteUnsuccessfulInteractionRequest.NewRequestFromMessage(
                interactionId: request.InteractionId,
                message: "Unable to complete the authentication request.  The bot does not have the necessary permissions on this server."
            ));
        }

    }
}
