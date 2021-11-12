using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Core.Requests.Interactions
{
    public class GrantRoleRequest : IRequest
    {
        public ulong InteractionId { get; init; }
        public string RequestedRole { get; init; }

        public static GrantRoleRequest CreateNewRequest(ulong interactionId, string requestedRole) =>
            new GrantRoleRequest
            {
                InteractionId = interactionId,
                RequestedRole = requestedRole
            };
    }
}
