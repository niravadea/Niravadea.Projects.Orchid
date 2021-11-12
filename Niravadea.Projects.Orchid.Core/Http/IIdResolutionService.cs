using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Core.Http
{
    public interface IIdResolutionService
    {
        Task<int> GetIdFromNameAsync(string name);
    }
}
