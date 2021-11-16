using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Client.Internal.Forum.Http
{
    public interface ITokenScraperService
    {
        Task<bool> CheckIfTokenExistsInProfile(string token, int id);
    }
}
