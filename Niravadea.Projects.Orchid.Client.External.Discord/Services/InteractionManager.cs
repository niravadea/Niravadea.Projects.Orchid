using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;
using Niravadea.Projects.Orchid.Client.External.Discord.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Client.External.Discord.Services
{
    public class InteractionManager : IInteractionManager
    {
        private class Interaction
        {
            private const double CullThresholdInSeconds = 15;
            private readonly InteractionContext context;
            private readonly CancellationTokenSource cancellationTokenSource;
            private readonly CancellationTokenRegistration cancellationTokenRegistration;

            // today I learned that method definitions are not compile time constants, apparently!  interesting.

            public Interaction(
                InteractionContext context,
                Action<object> timeoutCallback
            )
            {
                this.context = context;
                cancellationTokenSource = new CancellationTokenSource(
                    delay: TimeSpan.FromSeconds(CullThresholdInSeconds)
                );
                cancellationTokenRegistration = cancellationTokenSource.Token.Register(
                    callback: timeoutCallback,
                    state: this.context.InteractionId
                );
            }

            public async Task InitializeChannel()
            {
                await context.CreateResponseAsync(
                    type: InteractionResponseType.DeferredChannelMessageWithSource,
                    builder: new DiscordInteractionResponseBuilder().AsEphemeral(ephemeral: true)
                );
            }

            public async Task CompleteInteraction(ResultStatus result, string message)
            {
                string glyph = getGlyph(result);

                await context.EditResponseAsync(
                    builder: new DiscordWebhookBuilder().WithContent($"{glyph} {message}")
                );

                // unregister the timeout callback and just let it expire
                cancellationTokenRegistration.Unregister();
            }

            private string getGlyph(ResultStatus result) => result switch
            {
                ResultStatus.Successful => ":sunglasses:",
                ResultStatus.Failure => ":skull:",
                ResultStatus.Timeout => ":stopwatch:",
                _ => throw new InvalidOperationException()
            };

            internal async Task TimeoutInteraction()
            {
                string glyph = getGlyph(ResultStatus.Timeout);

                await context.EditResponseAsync(
                    builder: new DiscordWebhookBuilder().WithContent($"{glyph} Your request has timed out!")
                );
            }

            public async Task ApplyRole(string roleName)
            {
                DiscordRole roleActual = context.Guild.Roles.Values.SingleOrDefault(x => string.Equals(
                    a: x.Name,
                    b: roleName,
                    comparisonType: StringComparison.OrdinalIgnoreCase
                ));

                if (roleActual == null)
                {
                    throw new RoleNotFoundException(
                        roleName: roleName,
                        serverId: context.Guild.Id
                    );
                }

                DiscordMember specifiedMember = await context.Guild.GetMemberAsync(context.User.Id);

                bool memberIsInRoleActual = specifiedMember.Roles.Any(x => x.Id == (roleActual?.Id ?? ulong.MaxValue));

                if (!memberIsInRoleActual)
                {
                    await specifiedMember.GrantRoleAsync(
                        role: roleActual
                    );
                }
            }
        }

        private enum ResultStatus
        {
            Successful,
            Failure,
            Timeout
        }

        private readonly ILogger<InteractionManager> _logger;
        private readonly IDictionary<ulong, Interaction> _interactions = new Dictionary<ulong, Interaction>();

        public InteractionManager(ILogger<InteractionManager> logger)
        {
            _logger = logger;
        }


        public async Task CompleteInteractionAsFailure(ulong interactionId, string failureMessage) => await completeNormalInteraction(
            result: ResultStatus.Failure,
            interactionId: interactionId,
            message: failureMessage
        );

        public async Task CompleteInteractionAsSuccess(ulong interactionId, string successMessage) => await completeNormalInteraction(
            result: ResultStatus.Successful,
            interactionId: interactionId,
            message: successMessage
        );

        public async Task<ulong> RegisterNewInteraction(InteractionContext context)
        {
            _logger.LogTrace($"Registering interaction '{context.InteractionId}'");
            Interaction interaction = new Interaction(
                context: context,
                timeoutCallback: timeoutCallback
            );

            await interaction.InitializeChannel();

            _interactions.Add(context.InteractionId, interaction);

            return context.InteractionId;
        }

        private async Task completeNormalInteraction(ResultStatus result, ulong interactionId, string message)
        {
            if (!_interactions.ContainsKey(interactionId))
            {
                _logger.LogWarning($"Attempted to complete interaction '{interactionId}' but it was not found!");
                return;
            }

            await _interactions[interactionId].CompleteInteraction(
                result: result,
                message: message
            );
        }

        private async void timeoutCallback(object interactionId)
        {
            if (!ulong.TryParse(interactionId.ToString(), out ulong id))
            {
                _logger.LogError($"Interaction timeout was called, but no valid id was passed.  Passed value was '{interactionId}'");
                return;
            }

            // I know the general rule is to not use `async void`s because
            // exceptions can rise up and crash the program, but given that
            // an Action<object?> is the only available option for timeout
            // callbacks, this appears to be the only option.  so if it *is*
            // the only option, then wrapping the potential problematic code
            // in a try..catch shouuuuld be okay.
            try
            {
                await _interactions[id].TimeoutInteraction();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while removing interaction '{id}'");
            }
            finally
            {
                _interactions.Remove(id);
            }
        }

        public async Task AssignAuthenticatedRole(ulong interactionId)
        {
            if (!_interactions.ContainsKey(interactionId))
            {
                _logger.LogWarning($"Attempted to assign authenticated role on interaction '{interactionId}' but it was not found!");
                return;
            }

            Interaction interaction = _interactions[interactionId];

            await interaction.ApplyRole(Shared.Constants.AuthenticatedRole);
        }
    }
}
