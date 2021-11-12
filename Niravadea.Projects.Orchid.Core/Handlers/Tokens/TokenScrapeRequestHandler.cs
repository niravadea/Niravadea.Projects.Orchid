using MediatR;
using Niravadea.Projects.Orchid.Core.Http;
using Niravadea.Projects.Orchid.Core.Requests.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Core.Handlers.Tokens
{
    public class TokenScrapeRequestHandler : IRequestHandler<TokenScrapeRequest, bool>
    {
        private readonly ITokenScraperService _tokenScraperService;

        public TokenScrapeRequestHandler(ITokenScraperService tokenScraperService)
        {
            _tokenScraperService = tokenScraperService;
        }

        public async Task<bool> Handle(TokenScrapeRequest request, CancellationToken cancellationToken) =>
            await _tokenScraperService.CheckIfTokenExistsInProfile(
                token: request.Token,
                id: request.ForumsId
            );
    }
}
