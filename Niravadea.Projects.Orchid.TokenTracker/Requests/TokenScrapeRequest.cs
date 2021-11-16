using MediatR;

namespace Niravadea.Projects.Orchid.TokenTracker.Requests
{
    public class TokenScrapeRequest : IRequest<bool>
    {
        public ulong InteractionId { get; init; }
        public string Token { get; init; }
        public int ForumsId { get; set; }

        public static TokenScrapeRequest NewRequestFromContext(ulong interactionId, string token, int forumsId) =>
            new TokenScrapeRequest
            {
                InteractionId = interactionId,
                Token = token,
                ForumsId = forumsId
            };
    }
}
