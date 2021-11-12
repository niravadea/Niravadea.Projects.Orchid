using System.Threading.Tasks;
using System.Threading;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using MediatR.Pipeline;
using MediatR;
using Niravadea.Projects.Orchid.Core.Exceptions;
using Niravadea.Projects.Orchid.Core.Requests.Interactions;
using Niravadea.Projects.Orchid.Core.Requests.ForumLookups;

namespace Niravadea.Projects.Orchid.Core.Handlers.ForumLookups
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
