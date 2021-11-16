using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.TokenTracker.Requests
{
    public class TokenClearRequest : IRequest
    {
        public ulong DiscordId { get; init; }

        public static TokenClearRequest CreateNewRequestFromId(ulong discordId) => new TokenClearRequest
        {
            DiscordId = discordId
        };
    }
}
