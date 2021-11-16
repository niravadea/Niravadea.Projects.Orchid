using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Security.Cryptography;

namespace Niravadea.Projects.Orchid.Core
{
    public static class CoreComponentConfiguration
    {
        public static void ApplicationConfiguration(IConfigurationBuilder configuration)
        {

        }

        public static void ServiceConfiguration(HostBuilderContext context, IServiceCollection services)
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

        public static void LoggingConfiguration(ILoggingBuilder logging)
        {

        }
    }
}
