using MediatR;
using Niravadea.Projects.Orchid.Client.Internal.Forum.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Client.Internal.Forum.Handlers
{
    public class IdParseRequestHandler : IRequestHandler<IdParseRequest, int>
    {
        public Task<int> Handle(IdParseRequest request, CancellationToken cancellationToken) => Task.FromResult(int.Parse(request.UnparsedUserId));
    }
}
