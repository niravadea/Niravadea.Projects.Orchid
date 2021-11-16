using MediatR;

namespace Niravadea.Projects.Orchid.TokenTracker.Requests
{
    public class TokenGenerationRequest : IRequest<string>
    {
        public static TokenGenerationRequest NewRequest() => new TokenGenerationRequest();
    }
}
