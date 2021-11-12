using System.Threading.Tasks;
using MediatR;
using System.Threading;
using Niravadea.Projects.Orchid.Core.Http;
using Niravadea.Projects.Orchid.Core.Requests.ForumLookups;

namespace Niravadea.Projects.Orchid.Core.Handlers.ForumLookups
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
