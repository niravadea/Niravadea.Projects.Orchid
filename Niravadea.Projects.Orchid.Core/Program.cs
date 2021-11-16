using Microsoft.Extensions.Hosting;
using Niravadea.Projects.Orchid.AuthenticationDatabase;
using Niravadea.Projects.Orchid.AuthenticationDatabase.LiteDb;
using Niravadea.Projects.Orchid.Client.External.Discord;
using Niravadea.Projects.Orchid.Client.Internal.Forum;
using Niravadea.Projects.Orchid.Shared;
using Niravadea.Projects.Orchid.TokenTracker;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Core
{
    class Program
    {
        static async Task Main(string[] args) =>
            await Host
            .CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(CoreComponentConfiguration.ApplicationConfiguration)
            .ConfigureServices(CoreComponentConfiguration.ServiceConfiguration)
            .ConfigureServices(SharedComponentConfiguration.ServiceConfiguration)
            .ConfigureServices(InternalForumClientConfiguration.ServiceConfiguration)
            .ConfigureServices(ExternalDiscordClientConfiguration.ServiceConfiguration)
            .ConfigureServices(TokenTrackerConfiguration.ServiceConfiguration)
            .ConfigureServices(AuthenticationDatabaseConfiguration.ServiceConfiguration)
            .ConfigureServices(LiteDbAuthenticationDatabaseConfiguration.ServiceConfiguration)
            .ConfigureLogging(CoreComponentConfiguration.LoggingConfiguration)
            .Build()
            .RunAsync()
            ;
    }
}
