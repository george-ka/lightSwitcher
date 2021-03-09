 using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudCommandsReader
{
    public class CommandSender
    {
        public CommandSender(
            IHttpClientFactory clientFactory, 
            IOptions<CommandSenderSettings> settings,
            ILogger<CommandSender> logger)
        {
            if (settings == null || settings.Value == null)
            {
                throw new ArgumentNullException(nameof(settings));    
            }

            _settings = settings.Value;
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SendAsync(int command)
        {
            var client = _clientFactory.CreateClient();
            if (_settings.CommandMapping.Length <= command || command < 0)
            {
                _logger.LogError($"Command {command} is undefined");
                return;
            }

            var mappedCommand = _settings.CommandMapping[command];

            if (mappedCommand.Switch.HasValue)
            {
                await SendCommandAsync(mappedCommand.Switch.Value, mappedCommand.State, client);
            }
            else if (mappedCommand.Switches != null)
            {
                foreach (var switchNo in mappedCommand.Switches)
                {
                    await SendCommandAsync(switchNo, mappedCommand.State, client);
                }
            }
        }

        private async Task SendCommandAsync(int switchNo, SwitchState state, HttpClient client)
        {
            var message = new HttpRequestMessage(
                HttpMethod.Post,
                string.Format(
                    _settings.SwitcherUrlFormat, 
                    switchNo,
                    state.ToString().ToLower()));
            
            await client.SendAsync(message);
        }

        private readonly IHttpClientFactory _clientFactory;

        private readonly CommandSenderSettings _settings;

        private readonly ILogger<CommandSender> _logger;
    }
}