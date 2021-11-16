using Microsoft.Extensions.Options;
using Niravadea.Projects.Orchid.Client.Internal.Forum;
using Niravadea.Projects.Orchid.Client.Internal.Forum.Options;
using System.Net;
using System.Net.Http;

namespace Niravadea.Projects.Orchid.Client.Internal.Forum.Http
{
    public class ForumsAuthenticationHandler : HttpClientHandler
    {
        private readonly ForumsClientConfiguration _configuration;

        public ForumsAuthenticationHandler(IOptions<ForumsClientConfiguration> options)
        {
            _configuration = options.Value;
            CookieContainer = new CookieContainer(capacity: 4);
            CookieContainer.Add(new Cookie(name: "bbuserid", value: _configuration.UserId, path: "/", domain: Constants.ForumsUrl));
            CookieContainer.Add(new Cookie(name: "bbpassword", value: _configuration.HashedUserPassword, path: "/", domain: Constants.ForumsUrl));
            CookieContainer.Add(new Cookie(name: "sessionhash", value: _configuration.SessionHash, path: "/", domain: Constants.ForumsUrl));
            CookieContainer.Add(new Cookie(name: "sessionid", value: _configuration.SessionId, path: "/", domain: Constants.ForumsUrl));
        }
    }
}
