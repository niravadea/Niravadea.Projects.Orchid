using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Niravadea.Projects.Orchid.Core.Exceptions;

namespace Niravadea.Projects.Orchid.Core.Http
{
    public class IdResolutionService : IIdResolutionService
    {
        private readonly HttpClient _hc;
        private readonly ILogger<IdResolutionService> _logger;

        public IdResolutionService(
            HttpClient hc,
            ILogger<IdResolutionService> logger
        )
        {
            _hc = hc;
            _logger = logger;
        }

        // god I hate parsing HTML :D
        public async Task<int> GetIdFromNameAsync(string name)
        {
            string safeName = HttpUtility.UrlEncode(name);
            string uriString = $"https://{Constants.ForumsUrl}/member.php?action=getinfo&username={safeName}";
            Uri uri = new Uri(uriString);

            HttpResponseMessage response = await _hc.GetAsync(requestUri: uri);

            HtmlDocument document = new HtmlDocument();
            document.Load(stream: await response.Content.ReadAsStreamAsync());

            var errorNode = document.DocumentNode.SelectSingleNode("//body[@class='standarderror']");
            if (errorNode != null)
            {
                _logger.LogError($"Error while parsing page for user {name}");
                // idk.
                // TODO: make this more descriptive
                throw new UserSpecificException(name);
            }

            // check if we have a user profile
            var userInfoAnchor = document.DocumentNode.SelectSingleNode("//dl[@class='userinfo']");
            if (userInfoAnchor == null)
            {
                // no user data to parse
                throw new NonExistentUserException(name);
            }

            List<string>[] anchors = new List<string>[3];

            int expectedLinkAnchorCount = 2;
            var anchorAlpha = document.DocumentNode.SelectNodes("//span[@class='smalltext']/a");
            int actualLinkAnchorCount = anchorAlpha?.Count ?? 0;
            if (actualLinkAnchorCount == expectedLinkAnchorCount)
            {
                anchors[0] = (anchorAlpha?.Select(IdOrDefault) ?? Enumerable.Empty<string>()).ToList();
            }
            else
            {
                anchors[0] = new List<string>();
                _logger.LogWarning($"Found {actualLinkAnchorCount} link anchors while processing request for user '{name}'.  Was expecting {expectedLinkAnchorCount}!");
            }

            // this is the "Send a private message" link, if user has platinum
            var anchorBravo = document.DocumentNode.SelectSingleNode("//dl/dd/a[starts-with(@href, 'private.php')]");
            anchors[1] = anchorBravo != null ? new List<string> { IdOrDefault(anchorBravo) } : new List<string>();

            int expectedActionButtonCount = 2;
            // these are the "Add user to your {Buddy|Ignore} list" buttons at the bottom
            var anchorCharlie = document.DocumentNode.SelectNodes("//input[@name='userid']");
            int actualActionButtonCount = anchorCharlie?.Count ?? 0;
            if (actualLinkAnchorCount == expectedActionButtonCount)
            {
                anchors[2] = anchorCharlie.Select(x => x.GetAttributeValue(name: "value", def: "")).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            }
            else
            {
                anchors[2] = new List<string>();
                _logger.LogWarning($"Found {actualLinkAnchorCount} action buttons while processing request for user '{name}'.  Was expecting {expectedActionButtonCount}");
            }

            // flatten out all the values we found
            IEnumerable<string> flattenedAnchors = anchors.SelectMany(x => x);
            if (flattenedAnchors.Count() == 0)
            {
                _logger.LogError($"No user IDs for user {name}");
                throw new NoUserIdAnchorsAvailableException(name);
            }
            IEnumerable<string> distinctAnchors = flattenedAnchors.Distinct();

            // see if they're all the same
            bool allTheSame = distinctAnchors.Count() == 1;
            if (!allTheSame)
            {
                _logger.LogError($"Inconsistent user IDs for user {name}");
                throw new InconsistentUserIdException(name);
            }

            string potentialId = distinctAnchors.SingleOrDefault();

            return int.TryParse(s: potentialId, out int parsedId) ? parsedId : throw new UserSpecificException(potentialId);
        }

        private string IdOrDefault(HtmlNode node)
        {
            string data = node.GetAttributeValue(name: "href", def: "");
            int index = data.IndexOf(value: "userid=", comparisonType: StringComparison.OrdinalIgnoreCase);
            // this assumes that the userid=X key-value pair occurs at the end
            // of the query string.  if it doesn't, this will break hilariously.
            // probably fatally.  I should do something like a regex or
            // construct a Uri object.  I'll do that later, because it's 10pm,
            // I'm sleepy, and I want to go to bed.
            return index > 0 ? data.Substring(index + "userid=".Length) : default;
        }
    }
}
