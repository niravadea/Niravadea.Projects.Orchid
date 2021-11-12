using MediatR;
using Niravadea.Projects.Orchid.Core.Requests.Interactions;
using Niravadea.Projects.Orchid.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Core.Handlers.Interactions
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
