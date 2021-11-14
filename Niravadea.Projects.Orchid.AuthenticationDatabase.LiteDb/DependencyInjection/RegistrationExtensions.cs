using Microsoft.Extensions.DependencyInjection;
using Niravadea.Projects.Orchid.AuthenticationDatabase.Services;
using Implementations = Niravadea.Projects.Orchid.AuthenticationDatabase.LiteDb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using LiteDB;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Options;

namespace Niravadea.Projects.Orchid.AuthenticationDatabase.LiteDb.DependencyInjection
{
    public static class RegistrationExtensions
    {
        
        public static IServiceCollection AddAuthenticationDatabaseLiteDbImplementation(this IServiceCollection services) =>
            // this used to be .AddHostedService<Implementations.AuthenticationDatabase>()
            // we might consider making the IAuthenticationDatabase interface
            // inherit the MediatR.IRequestHandler<,> interfaces, that way, we
            // could make the service itself handle those requests, vs.
            // resolving a reference to the service, then calling the
            // appropriate methods.
            services.AddSingleton<IAuthenticationDatabase, Implementations.AuthenticationDatabase>()
                    .AddSingleton<ILiteDatabase, LiteDatabase>(provider => {
                        // TODO: see if there's a cleaner way to do this -
                        // while keeping this extension out of the main
                        // project, of course.
                        IConfiguration configurationProvider = provider.GetService<IConfiguration>();
                        LiteDbConfiguration configuration = new LiteDbConfiguration();
                        configurationProvider.Bind("LiteDbConfiguration", configuration);

                        return new LiteDatabase(new ConnectionString()
                        {
                            Filename = configuration.Filename,
                            Password = configuration.Password
                        });
                    });
    }
}
