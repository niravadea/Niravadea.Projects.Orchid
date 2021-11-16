using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Client.External.Discord.Exceptions
{
    public class RoleNotFoundException : Exception
    {
        public RoleNotFoundException(string roleName, ulong serverId)
            : base(message: $"Role {roleName} not found on server {serverId}.") { }
    }
}
