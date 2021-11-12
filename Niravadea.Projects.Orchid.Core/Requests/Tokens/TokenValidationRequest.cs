using MediatR;

namespace Niravadea.Projects.Orchid.Core.Requests.Tokens
{
    public class TokenValidationRequest : IRequest<Unit>
    {
        public ulong InteractionId { get; init; }

        public ulong UserId { get; init; }

        public static TokenValidationRequest NewRequestFromId(ulong interactionId, ulong userId) =>
            new TokenValidationRequest
            {
                InteractionId = interactionId,
                UserId = userId
            };
    }
}
