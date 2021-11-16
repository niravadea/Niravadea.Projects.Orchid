using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;

namespace Niravadea.Projects.Orchid.Client.External.Discord.Services
{
    public class BotService : IHostedService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly DiscordClient _client;

        public BotService(
            IHostApplicationLifetime hostApplicationLifetime,
            DiscordClient client
        )
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _client = client;
        }

        public async Task StartAsync(CancellationToken cancellationToken) => await _client.ConnectAsync();

        public async Task StopAsync(CancellationToken cancellationToken) => await _client.DisconnectAsync();
    }
}
