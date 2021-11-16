using MediatR;
using Niravadea.Projects.Orchid.Client.External.Discord.Services;
using Niravadea.Projects.Orchid.Shared.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Client.External.Discord.Handlers
{
    public class GrantRoleRequestHandler : IRequestHandler<GrantRoleRequest>
    {
        private readonly IInteractionManager _manager;

        public GrantRoleRequestHandler(IInteractionManager manager)
        {
            _manager = manager;
        }

        public async Task<Unit> Handle(GrantRoleRequest request, CancellationToken cancellationToken)
        {
            await _manager.AssignAuthenticatedRole(request.InteractionId);
            return Unit.Value;
        }
    }
}
