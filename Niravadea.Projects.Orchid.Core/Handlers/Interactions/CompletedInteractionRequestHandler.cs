using MediatR;
using Niravadea.Projects.Orchid.Core.Requests.Interactions;
using Niravadea.Projects.Orchid.Core.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Core.Handlers.Interactions
{
    public class CompletedInteractionRequestHandler
        : IRequestHandler<CompleteSuccessfulInteractionRequest>
        , IRequestHandler<CompleteUnsuccessfulInteractionRequest>
    {
        private readonly IInteractionManager _interactionManager;

        public CompletedInteractionRequestHandler(IInteractionManager interactionManager)
        {
            _interactionManager = interactionManager;
        }

        public async Task<Unit> Handle(CompleteSuccessfulInteractionRequest request, CancellationToken cancellationToken)
        {
            await _interactionManager.CompleteInteractionAsSuccess(
                interactionId: request.InteractionId,
                successMessage: request.Message
            );
            return Unit.Value;
        }

        public async Task<Unit> Handle(CompleteUnsuccessfulInteractionRequest request, CancellationToken cancellationToken)
        {
            await _interactionManager.CompleteInteractionAsFailure(
                interactionId: request.InteractionId,
                failureMessage: request.Message
            );
            return Unit.Value;
        }
    }
}
