using DSharpPlus.SlashCommands;
using MediatR;

namespace Niravadea.Projects.Orchid.Core.Requests.Interactions
{
    public class RegisterNewInteractionRequest : IRequest<ulong>
    {
        public InteractionContext Context { get; init; }

        public static RegisterNewInteractionRequest NewRequest(InteractionContext context) =>
            new RegisterNewInteractionRequest
            {
                Context = context
            };
    }
}
