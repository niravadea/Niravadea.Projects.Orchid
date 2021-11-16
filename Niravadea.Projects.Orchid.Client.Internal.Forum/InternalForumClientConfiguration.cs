using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Niravadea.Projects.Orchid.Client.Internal.Forum.Options;
using Niravadea.Projects.Orchid.Client.Internal.Forum.Http;
using MediatR;
using System.Reflection;

namespace Niravadea.Projects.Orchid.Client.Internal.Forum
{
    public static class InternalForumClientConfiguration
    {
        public static void ServiceConfiguration(HostBuilderContext context, IServiceCollection services)
        {
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

            // and the one for the client that will be resolving forums data
            services.Configure<ForumsClientConfiguration>(
                config: context.Configuration.GetSection("SaClientConfiguration"),
                configureBinder: options => options.BindNonPublicProperties = true
            );


            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
