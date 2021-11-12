using MediatR;

namespace Niravadea.Projects.Orchid.Core.Requests.Interactions
{
    public class CompleteUnsuccessfulInteractionRequest : IRequest
    {
        public ulong InteractionId { get; init; }
        public string Message { get; init; }

        public static CompleteUnsuccessfulInteractionRequest NewRequestFromMessage(ulong interactionId, string message) =>
            new CompleteUnsuccessfulInteractionRequest
            {
                InteractionId = interactionId,
                Message = message
            };
    }
}
