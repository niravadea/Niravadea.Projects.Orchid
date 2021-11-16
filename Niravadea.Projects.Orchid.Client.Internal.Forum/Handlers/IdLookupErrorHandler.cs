using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using MediatR.Pipeline;
using MediatR;
using Niravadea.Projects.Orchid.Shared.Requests;
using Niravadea.Projects.Orchid.Client.Internal.Forum.Requests;
using Niravadea.Projects.Orchid.Client.Internal.Forum.Exceptions;

namespace Niravadea.Projects.Orchid.Client.Internal.Forum.Handlers
{
    public class IdLookupErrorHandler
        : IRequestExceptionAction<IdLookupRequest, InconsistentUserIdException>
        , IRequestExceptionAction<IdLookupRequest, NoUserIdAnchorsAvailableException>
        , IRequestExceptionAction<IdLookupRequest, UserSpecificException>
        , IRequestExceptionAction<IdLookupRequest, NonExistentUserException>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<IdLookupErrorHandler> _logger;

        public IdLookupErrorHandler(IMediator mediator, ILogger<IdLookupErrorHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Execute(IdLookupRequest request, InconsistentUserIdException exception, CancellationToken cancellationToken)
            => await UserNameErrorHandler(request, exception);

        public async Task Execute(IdLookupRequest request, NoUserIdAnchorsAvailableException exception, CancellationToken cancellationToken)
            => await UserNameErrorHandler(request, exception);

        public async Task Execute(IdLookupRequest request, UserSpecificException exception, CancellationToken cancellationToken)
            => await UserNameErrorHandler(request, exception);

        public async Task Execute(IdLookupRequest request, NonExistentUserException exception, CancellationToken cancellationToken)
            => await UserNameErrorHandler(request, exception);

        private async Task UserNameErrorHandler(IdLookupRequest request, UserSpecificException exception)
        {
            _logger.LogError(exception, $"Unable to parse user page '{request.UserName}'");
            await _mediator.Send(CompleteUnsuccessfulInteractionRequest.NewRequestFromMessage(
                interactionId: request.InteractionId,
                message: "An error occurred while parsing your user page.  Please retry with your user ID instead."
            ));
        }
    }
}
