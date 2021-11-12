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
    public class AuthenticationCommitRequestHandler : IRequestHandler<AuthenticationCommitRequest, bool>
    {
        private readonly IAuthenticationDatabase _authenticationDatabase;

        public AuthenticationCommitRequestHandler(IAuthenticationDatabase authenticationDatabase)
        {
            _authenticationDatabase = authenticationDatabase;
        }

        public async Task<bool> Handle(AuthenticationCommitRequest request, CancellationToken cancellationToken) => await _authenticationDatabase.WriteUserAuthentication(request.DiscordId, request.ForumsId);
    }
}
