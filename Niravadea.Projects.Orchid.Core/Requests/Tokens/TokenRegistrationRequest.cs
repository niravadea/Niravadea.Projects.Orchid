using MediatR;

namespace Niravadea.Projects.Orchid.Core.Requests.Tokens
{
    public class TokenRegistrationRequest : IRequest<Unit>
    {
        public int ForumsId { get; init; }

        public ulong InteractionId { get; init; }
        public ulong UserId { get; init; }
        public ulong GuildId { get; init; }

        public static TokenRegistrationRequest NewRequestFromContext(ulong interactionId, ulong guildId, ulong userId, int forumsId) =>
            new TokenRegistrationRequest
            {
                ForumsId = forumsId,
                InteractionId = interactionId,
                UserId = userId,
                GuildId = guildId
            };
    }
}
