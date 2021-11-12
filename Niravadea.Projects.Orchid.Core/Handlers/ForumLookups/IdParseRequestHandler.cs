using MediatR;
using Niravadea.Projects.Orchid.Core.Requests.ForumLookups;
using System.Threading;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Core.Handlers.ForumLookups
{
    public class IdParseRequestHandler : IRequestHandler<IdParseRequest, int>
    {
        public Task<int> Handle(IdParseRequest request, CancellationToken cancellationToken) => Task.FromResult(int.Parse(request.UnparsedUserId));
    }
}
