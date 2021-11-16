using DSharpPlus;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Niravadea.Projects.Orchid.Client.External.Discord.Services;
using Niravadea.Projects.Orchid.Client.External.Discord.Options;
using MediatR;
using System.Reflection;

namespace Niravadea.Projects.Orchid.Client.External.Discord
{
    public static class ExternalDiscordClientConfiguration
    {
        public static void ServiceConfiguration(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton(provider =>
            {
                // get the options we'll need
                IOptions<DiscordClientConfiguration> options = provider.GetRequiredService<IOptions<DiscordClientConfiguration>>();

                // create the client
                DiscordClient client = new DiscordClient(new DiscordConfiguration
                {
                    Token = options.Value.Token,
                    TokenType = TokenType.Bot
                });

                SlashCommandsConfiguration slashCommandsConfiguration = new SlashCommandsConfiguration { Services = provider };

                // configure our client for slash commands and register our commands
                // this doesn't actually do anything until the client connects to discord
                client.UseSlashCommands(config: slashCommandsConfiguration)
                    .RegisterCommands<Commands>(guildId: options.Value.TestingGuildId)
                    ;

                return client;
            })
            .Configure<DiscordClientConfiguration>(
                config: context.Configuration.GetSection("BotClientConfiguration")
            )
            .AddHostedService<BotService>()
            .AddSingleton<IInteractionManager, InteractionManager>()

            .AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
