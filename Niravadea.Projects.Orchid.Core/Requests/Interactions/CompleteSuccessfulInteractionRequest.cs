﻿using MediatR;

namespace Niravadea.Projects.Orchid.Core.Requests.Interactions
{
    public class CompleteSuccessfulInteractionRequest : IRequest
    {
        public ulong InteractionId { get; init; }
        public string Message { get; init; }

        public static CompleteSuccessfulInteractionRequest NewRequestFromMessage(ulong interactionId, string message) =>
            new CompleteSuccessfulInteractionRequest
            {
                InteractionId = interactionId,
                Message = message
            };
    }
}
