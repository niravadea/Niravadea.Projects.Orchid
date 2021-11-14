using DSharpPlus;
using DSharpPlus.SlashCommands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Niravadea.Projects.Orchid.AuthenticationDatabase.DependencyInjection;
using Niravadea.Projects.Orchid.AuthenticationDatabase.LiteDb.DependencyInjection;
using Niravadea.Projects.Orchid.Core.Discord;
using Niravadea.Projects.Orchid.Core.Http;
using Niravadea.Projects.Orchid.Core.Options;
using Niravadea.Projects.Orchid.Core.Services;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Core
{
    class Program
    {
        static async Task Main(string[] args) =>
            await Host
            .CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(ApplicationConfiguration)
            .ConfigureServices(ServiceConfiguration)
            .ConfigureLogging(LoggingConfiguration)
            .Build()
            .RunAsync()
            ;

        private static void ApplicationConfiguration(IConfigurationBuilder configuration)
        {

        }

        private static void ServiceConfiguration(HostBuilderContext context, IServiceCollection services)
        {
            // add IOptions<T> processing
            services.AddOptions();

            // add the mediator
            services
                .AddMediatR(Assembly.GetEntryAssembly())
                .AddAuthenticationDatabaseRequests();

            // https://docs.microsoft.com/en-us/dotnet/api/system.net.httpwebrequest?view=net-5.0
            // We don't recommend that you use HttpWebRequest for new development. Instead, use the System.Net.Http.HttpClient class.

            // https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0
            // HttpClient is intended to be instantiated once and re-used throughout the life of an application. Instantiating an HttpClient class for every request will exhaust the number of sockets available under heavy loads

            // https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            // A Typed Client is effectively a transient object, that means a new instance is created each time one is needed. It receives a new HttpClient instance each time it's constructed.
            // However, the HttpMessageHandler objects in the pool are the objects that are reused by multiple HttpClient instances.

            // The HttpClient instances injected by DI, can be disposed of safely, because the associated HttpMessageHandler is managed by the factory. As a matter of fact, injected HttpClient instances are Scoped from a DI perspective.

            services.AddTransient<ForumsAuthenticationHandler>();

            // you can pass an System.Net.Http.HttpMessageHandler into the HttpClient constructor
            services
                .AddHttpClient<IIdResolutionService, IdResolutionService>() // <-- *these* are transient in nature.  their message handlers are pooled.  THOSE are the ones that lead to socket exhaustion
                .ConfigureHttpClient(client => client.BaseAddress = new Uri($"https://{Constants.ForumsUrl}"))
                .ConfigurePrimaryHttpMessageHandler<ForumsAuthenticationHandler>()
                .SetHandlerLifetime(TimeSpan.FromSeconds(5))
                ;

            services
                .AddHttpClient<ITokenScraperService, TokenScraperService>()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri($"https://{Constants.ForumsUrl}"))
                .ConfigurePrimaryHttpMessageHandler<ForumsAuthenticationHandler>()
                .SetHandlerLifetime(TimeSpan.FromSeconds(5))
                ;
            // TODO: either unify the above services, or make an extension to do the necessary configurations

            // register services
            services
                // register RNG service
                .AddSingleton<RandomNumberGenerator, RNGCryptoServiceProvider>()
                // register the token tracker
                .AddSingleton<ITokenTracker, TokenTracker>()
                .AddSingleton<IInteractionManager, InteractionManager>()
                // add instance for our Discord client
                .AddSingleton(provider =>
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
                    });

            // configure the IOptions<ClientConfiguration> injection
            services.Configure<DiscordClientConfiguration>(
                config: context.Configuration.GetSection("BotClientConfiguration")
            );

            // and the one for the client that will be resolving forums data
            services.Configure<ForumsClientConfiguration>(
                config: context.Configuration.GetSection("SaClientConfiguration"),
                configureBinder: options => options.BindNonPublicProperties = true
            );

            // add hosted services
            services
                .AddHostedService<BotService>()
                .AddAuthenticationDatabaseLiteDbImplementation();
        }

        private static void LoggingConfiguration(ILoggingBuilder logging)
        {

        }
    }
}
