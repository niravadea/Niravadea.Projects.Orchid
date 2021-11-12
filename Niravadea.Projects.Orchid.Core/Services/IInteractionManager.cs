using DSharpPlus.SlashCommands;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Core.Services
{
    public interface IInteractionManager
    {
        Task<ulong> RegisterNewInteraction(InteractionContext context);
        Task CompleteInteractionAsSuccess(ulong interactionId, string successMessage);
        Task CompleteInteractionAsFailure(ulong interactionId, string failureMessage);
        Task AssignAuthenticatedRole(ulong interactionId);
    }
}
