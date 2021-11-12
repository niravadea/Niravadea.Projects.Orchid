using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.AuthenticationDatabase.Requests
{
    public class AuthenticationCommitRequest : IRequest<bool>
    {
        public int ForumsId { get; init; }
        public ulong DiscordId { get; init; }
    }
}
