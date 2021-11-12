using MediatR;

namespace Niravadea.Projects.Orchid.Core.Requests.Tokens
{
    public class TokenGenerationRequest : IRequest<string>
    {
        public static TokenGenerationRequest NewRequest() => new TokenGenerationRequest();
    }
}
