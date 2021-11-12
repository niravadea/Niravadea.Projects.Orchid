using Microsoft.Extensions.Hosting;
using Niravadea.Projects.Orchid.AuthenticationDatabase.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace Niravadea.Projects.Orchid.AuthenticationDatabase.LiteDb.Services
{
    public class AuthenticationDatabase
        : IAuthenticationDatabase
    {
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly ILiteDatabase _database;
        private readonly ILiteCollection<AuthenticationRecord> _authenticationEntries;
        private readonly ILogger<AuthenticationDatabase> _logger;

        private const string ForumsIdKeyName = "AK_ForumsId";
        private const string DiscordIdKeyName = "AK_DiscordId";

        public AuthenticationDatabase(
            ILiteDatabase database,
            ILogger<AuthenticationDatabase> logger
        )
        {
            _database = database;
            _logger = logger;
            _authenticationEntries = _database.GetCollection<AuthenticationRecord>(name: nameof(AuthenticationRecord));

            if (_authenticationEntries.EnsureIndex(name: ForumsIdKeyName, keySelector: x => x.ForumsId))
            {
                _logger.LogInformation($"Generated index for key '{ForumsIdKeyName}'");
            }

            if (_authenticationEntries.EnsureIndex(name: DiscordIdKeyName, keySelector: x => x.DiscordId))
            {
                _logger.LogInformation($"Generated index for key '{DiscordIdKeyName}'");
            }
        }

        public Task<bool> CheckUserAuthenticationAsync(ulong discordId) => Task.FromResult(_authenticationEntries.Count(x => x.DiscordId == discordId) > 0);

        public Task<bool> WriteUserAuthentication(ulong discordId, int forumsId) {

            BsonValue result = _authenticationEntries
            .Insert(new AuthenticationRecord
            {
                DiscordId = discordId,
                ForumsId = forumsId
            });

            // TODO: figure out how to better handle this.
            return Task.FromResult(!string.IsNullOrWhiteSpace(result.AsObjectId.ToString()));
        }
    }
}
