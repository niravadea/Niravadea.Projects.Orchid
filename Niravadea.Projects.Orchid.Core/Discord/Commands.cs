using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using MediatR;
using Microsoft.Extensions.Logging;
using Niravadea.Projects.Orchid.AuthenticationDatabase.Requests;
using Niravadea.Projects.Orchid.Core.Requests.ForumLookups;
using Niravadea.Projects.Orchid.Core.Requests.Interactions;
using Niravadea.Projects.Orchid.Core.Requests.Tokens;

namespace Niravadea.Projects.Orchid.Core.Discord
{
    // none of these actually get registered or created until we call the
    // .ConnectAsync method on the client
    // also, changes get updated automatically
    // also also, this is registered as a transient service
    public class Commands : ApplicationCommandModule
    {
        private readonly IMediator _mediator;
        private readonly ILogger<Commands> _logger;

        public Commands(
            IMediator mediator,
            ILogger<Commands> logger
        )
        {
            _mediator = mediator;
            // feel like this should be handled via mediator, if we really wanted to go all out.
            _logger = logger;
        }

#if DEBUG
        [SlashCommand("debug", "Debug friend")]
        public async Task DoAThing(InteractionContext context)
        {
            ulong a = await _mediator.Send(RegisterNewInteractionRequest.NewRequest(context));
            await _mediator.Send(CompleteSuccessfulInteractionRequest.NewRequestFromMessage(a, "test"));
        }

        [SlashCommand("eggplant", "Get an eggplant")]
        public async Task TestCommand(InteractionContext ctx)
        {
            var type = InteractionResponseType.ChannelMessageWithSource;

            var builder = new DiscordInteractionResponseBuilder().WithContent(Glyphs.EggplantGlyph);

            await ctx.CreateResponseAsync(
                type: type,
                builder: builder
            );
        }

        [SlashCommand("peggplant", "Get a private eggplant")]
        public async Task PrivateTestCommand(InteractionContext context)
        {
            ulong id = await _mediator.Send(RegisterNewInteractionRequest.NewRequest(
                context: context
            ));

            await Task.Delay(500);

            await _mediator.Send(CompleteSuccessfulInteractionRequest.NewRequestFromMessage(
                interactionId: id,
                message: Glyphs.EggplantGlyph
            ));
        }
#endif

        [SlashCommand("authenticate", "Begin a new authentication process")]
        public async Task AuthenticateAsync(
            InteractionContext context,
            [Option(name: "method", description: "How you want to authenticate")] AuthType authType,
            [Option(name: "value", description: "ID that you just selected")] string id)
        {
            //await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
            ulong interactionId = await _mediator.Send(RegisterNewInteractionRequest.NewRequest(context));

            // TODO: handle error return codes here.  or do an exception
            // catcher and use those?  idk.  research: "C# errors:
            // exceptions vs error codes", also probably want to ponder
            // if malicious or garbage entries are "expected".  Kinda feel
            // like they should be.

            bool isAlreadyKnown = await _mediator.Send(new AuthenticationCheckRequest { DiscordId = context.User.Id });

            if (isAlreadyKnown)
            {
                /*
                await _mediator.Send(CompleteSuccessfulInteractionRequest.NewRequestFromMessage(
                    interactionId: interactionId,
                    message: "You've already authenticated!"
                ));
                */

                // handle role assignments if not already done

                await _mediator.Send(GrantRoleRequest.CreateNewRequest(
                    interactionId: interactionId,
                    requestedRole: Constants.AuthenticatedRole
                ));
                return;
            }

            int targetId = authType switch
            {
                AuthType.ByUserId => await _mediator.Send(IdParseRequest.NewRequestFromContext(
                    interactionId: context.InteractionId,
                    userId: id
                )),
                AuthType.ByUserName => await _mediator.Send(IdLookupRequest.NewRequestFromContext(
                    interactionId: context.InteractionId,
                    userName: id
                )),
                _ => throw new InvalidOperationException()
            };

            await _mediator.Send(TokenRegistrationRequest.NewRequestFromContext(
                interactionId: context.InteractionId,
                guildId: context.Guild.Id,
                userId: context.User.Id,
                forumsId: targetId
            ));
        }

        [SlashCommand("validate", "Complete an already started authentication process")]
        public async Task ValidateAsync(
            InteractionContext context
        )
        {
            ulong interactionId = await _mediator.Send(RegisterNewInteractionRequest.NewRequest(
                context: context
            ));

            await _mediator.Send(TokenValidationRequest.NewRequestFromId(
                interactionId: interactionId,
                userId: context.User.Id
            ));
        }

        [SlashCommand("clear", "Clear any pending tokens for your Discord ID")]
        public async Task ClearPendingToken(
            InteractionContext context
        )
        {
            ulong interactionId = await _mediator.Send(RegisterNewInteractionRequest.NewRequest(
                context: context
            ));

            await _mediator.Send(TokenClearRequest.CreateNewRequestFromId(
                discordId: context.User.Id
            ));

            await _mediator.Send(CompleteSuccessfulInteractionRequest.NewRequestFromMessage(
                interactionId: interactionId,
                message: "Cleared all pending tokens associated with your Discord ID!"
            ));
        }
    }
}
