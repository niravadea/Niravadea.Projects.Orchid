using DSharpPlus.SlashCommands;
using MediatR;

namespace Niravadea.Projects.Orchid.Client.External.Discord.Requests
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
