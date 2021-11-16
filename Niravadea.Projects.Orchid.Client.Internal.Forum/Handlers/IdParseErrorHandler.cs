using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using System;
using MediatR.Pipeline;
using MediatR;
using Niravadea.Projects.Orchid.Shared.Requests;
using Niravadea.Projects.Orchid.Client.Internal.Forum.Requests;

namespace Niravadea.Projects.Orchid.Client.Internal.Forum.Handlers
{
    public class IdParseErrorHandler
        : IRequestExceptionAction<IdParseRequest, ArgumentNullException>
        , IRequestExceptionAction<IdParseRequest, ArgumentException>
        , IRequestExceptionAction<IdParseRequest, FormatException>
        , IRequestExceptionAction<IdParseRequest, OverflowException>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<IdParseErrorHandler> _logger;

        public IdParseErrorHandler(IMediator mediator, ILogger<IdParseErrorHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }


        public async Task Execute(IdParseRequest request, ArgumentNullException exception, CancellationToken cancellationToken)
            => await UserIdErrorHandler(request, exception);

        public async Task Execute(IdParseRequest request, ArgumentException exception, CancellationToken cancellationToken)
            => await UserIdErrorHandler(request, exception);

        public async Task Execute(IdParseRequest request, FormatException exception, CancellationToken cancellationToken)
            => await UserIdErrorHandler(request, exception);

        public async Task Execute(IdParseRequest request, OverflowException exception, CancellationToken cancellationToken)
            => await UserIdErrorHandler(request, exception);

        private async Task UserIdErrorHandler(IdParseRequest request, Exception exception)
        {
            _logger.LogError(exception, $"Unable to parse user id '{request.UnparsedUserId}'");
            await _mediator.Send(CompleteUnsuccessfulInteractionRequest.NewRequestFromMessage(
                interactionId: request.InteractionId,
                message: "An error occurred while parsing your user id.  Request has been cancelled."
            ));
        }
    }
}
