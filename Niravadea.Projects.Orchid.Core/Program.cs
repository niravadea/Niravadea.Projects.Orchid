using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Niravadea.Projects.Orchid.AuthenticationDatabase;
using Niravadea.Projects.Orchid.AuthenticationDatabase.LiteDb;
using Niravadea.Projects.Orchid.Client.External.Discord;
using Niravadea.Projects.Orchid.Client.Internal.Forum;
using Niravadea.Projects.Orchid.Shared;
using Niravadea.Projects.Orchid.TokenTracker;
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
            .ConfigureServices(SharedComponentConfiguration.ServiceConfiguration)
            .ConfigureServices(InternalForumClientConfiguration.ServiceConfiguration)
            .ConfigureServices(ExternalDiscordClientConfiguration.ServiceConfiguration)
            .ConfigureServices(TokenTrackerConfiguration.ServiceConfiguration)
            .ConfigureServices(AuthenticationDatabaseConfiguration.ServiceConfiguration)
            .ConfigureServices(LiteDbAuthenticationDatabaseConfiguration.ServiceConfiguration)
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
                .AddMediatR(Assembly.GetEntryAssembly());

            // register services
            services
                // register RNG service
                .AddSingleton<RandomNumberGenerator, RNGCryptoServiceProvider>();
        }

        private static void LoggingConfiguration(ILoggingBuilder logging)
        {

        }
    }
}
