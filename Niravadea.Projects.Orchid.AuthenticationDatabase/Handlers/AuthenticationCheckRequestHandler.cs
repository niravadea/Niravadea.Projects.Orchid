using MediatR;
using Niravadea.Projects.Orchid.AuthenticationDatabase.Requests;
using Niravadea.Projects.Orchid.AuthenticationDatabase.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.AuthenticationDatabase.Handlers
{
    public class AuthenticationCheckRequestHandler : IRequestHandler<AuthenticationCheckRequest, bool>
    {
        private readonly IAuthenticationDatabase _authenticationDatabase;

        public AuthenticationCheckRequestHandler(IAuthenticationDatabase authenticationDatabase)
        {
            _authenticationDatabase = authenticationDatabase;
        }

        public async Task<bool> Handle(AuthenticationCheckRequest request, CancellationToken cancellationToken) => await _authenticationDatabase.CheckUserAuthenticationAsync(request.DiscordId);
    }
}
