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

        public async Task Execute(GrantRoleRequest request, RoleNotFoundException exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An error occurred while trying to grant an authenticated role.");
            await _mediator.Send(CompleteUnsuccessfulInteractionRequest.NewRequestFromMessage(
                interactionId: request.InteractionId,
                message: "Completed authentication, but the server does not have the necessary role!  Please `/authenticate` again after the server adds the necessary role."
            ));
        }
    }
}
