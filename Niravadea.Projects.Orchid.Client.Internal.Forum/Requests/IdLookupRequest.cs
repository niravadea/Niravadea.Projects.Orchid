using MediatR;

namespace Niravadea.Projects.Orchid.Client.Internal.Forum.Requests
{
    public class IdLookupRequest : IRequest<int>
    {
        public string UserName { get; init; }
        public ulong InteractionId { get; init; }

        public static IdLookupRequest NewRequestFromContext(ulong interactionId, string userName) =>
            new IdLookupRequest
            {
                InteractionId = interactionId,
                UserName = userName
            };
    }
}
