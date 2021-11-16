using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Niravadea.Projects.Orchid.Client.Internal.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Client.Internal.Forum.Http
{
    public class TokenScraperService
        : ITokenScraperService
    {
        private readonly HttpClient _hc;
        private readonly ILogger<TokenScraperService> _logger;

        public TokenScraperService(
            HttpClient hc,
            ILogger<TokenScraperService> logger
        )
        {
            _hc = hc;
            _logger = logger;
        }

        public async Task<bool> CheckIfTokenExistsInProfile(string token, int id)
        {
            // https://forums.somethingawful.com/member.php?action=getinfo&userid=39922
            string uriString = $"https://{Constants.ForumsUrl}/member.php?action=getinfo&userid={id}";
            Uri uri = new Uri(uriString);

            HttpResponseMessage response = await _hc.GetAsync(requestUri: uri);

            HtmlDocument document = new HtmlDocument();
            document.Load(stream: await response.Content.ReadAsStreamAsync());

            return document.ParsedText.IndexOf(token) > 0;
        }
    }
}
