using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Niravadea.Projects.Orchid.TokenTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.TokenTracker
{
    public static class TokenTrackerConfiguration
    {
        public static void ServiceConfiguration(IServiceCollection services)
        {
            // register the token tracker
            services
                .AddSingleton<ITokenTracker, Services.TokenTracker>()
                .AddMediatR(Assembly.GetExecutingAssembly()); ;
        }
    }
}
