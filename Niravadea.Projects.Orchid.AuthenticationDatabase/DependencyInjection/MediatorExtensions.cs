using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using System.Reflection;

namespace Niravadea.Projects.Orchid.AuthenticationDatabase.DependencyInjection
{
    public static class MediatorExtensions
    {
        public static IServiceCollection AddAuthenticationDatabaseRequests(this IServiceCollection services) => services.AddMediatR(Assembly.GetExecutingAssembly());
    }
}
