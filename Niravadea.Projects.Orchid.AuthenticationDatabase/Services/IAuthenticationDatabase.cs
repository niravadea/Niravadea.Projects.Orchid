using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.AuthenticationDatabase.Services
{
    // This is the authentication database that will contain the:
    //
    //  1. authentication ID (identity PK)
    //      int     32 bits
    //  2. discord user id of the original user
    //      ulong   64 bits
    //  3. forums user id of the authenticated user
    //      int     32 bits
    //
    //  each record should be ~128-bits / 16 bytes
    //  assuming 205802 users - as of 2021-10-25 - the maximum size would be in the neighborhood of 3.2MB.  not too bad.
    //
    // We can't use the discord ID as a PK, because one forums user may have multiple discord accounts, or create a new one.
    // We can't use the forums ID as a PK, because one discord user may have multiple forums accounts, or create a new one.
    public interface IAuthenticationDatabase
    {
        Task<bool> CheckUserAuthenticationAsync(ulong discordId);

        Task<bool> WriteUserAuthentication(ulong discordId, int forumsId);
    }
}
