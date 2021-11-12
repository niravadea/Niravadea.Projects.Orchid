using MediatR;

namespace Niravadea.Projects.Orchid.Core.Requests.ForumLookups
{
    public class IdParseRequest : IRequest<int>
    {
        public string UnparsedUserId { get; init; }
        public ulong InteractionId { get; init; }

        public static IdParseRequest NewRequestFromContext(ulong interactionId, string userId) =>
            new IdParseRequest
            {
                InteractionId = interactionId,
                UnparsedUserId = userId
            };
    }
}
