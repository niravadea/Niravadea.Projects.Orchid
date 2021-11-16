using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Client.Internal.Forum.Http
{
    public interface IIdResolutionService
    {
        Task<int> GetIdFromNameAsync(string name);
    }
}
