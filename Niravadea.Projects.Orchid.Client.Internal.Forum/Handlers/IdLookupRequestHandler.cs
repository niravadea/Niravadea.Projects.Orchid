using System.Threading.Tasks;
using MediatR;
using System.Threading;
using Niravadea.Projects.Orchid.Client.Internal.Forum.Requests;
using Niravadea.Projects.Orchid.Client.Internal.Forum.Http;

namespace Niravadea.Projects.Orchid.Client.Internal.Forum.Handlers
{
    public class IdLookupRequestHandler : IRequestHandler<IdLookupRequest, int>
    {
        private readonly IIdResolutionService _service;

        public IdLookupRequestHandler(IIdResolutionService service)
        {
            _service = service;
        }

        public async Task<int> Handle(IdLookupRequest request, CancellationToken cancellationToken) =>
            await _service.GetIdFromNameAsync(
                name: request.UserName
            );
    }
}
