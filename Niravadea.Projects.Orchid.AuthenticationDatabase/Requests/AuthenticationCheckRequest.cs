using MediatR;

namespace Niravadea.Projects.Orchid.AuthenticationDatabase.Requests
{
    public class AuthenticationCheckRequest : IRequest<bool>
    {
        public ulong DiscordId { get; init; }
    }
}
