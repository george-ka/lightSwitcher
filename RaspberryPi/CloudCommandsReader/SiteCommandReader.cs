using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudCommandsReader
{
    public class SiteCommandReader : ICommandReader
    {
        public SiteCommandReader(
            IOptions<SiteCommandReaderSettings> settingsOptions,
            IHttpClientFactory clientFactory, 
            ILogger<SiteCommandReader> logger)
        {
            if (settingsOptions == null || settingsOptions.Value == null)
            {
                throw new ArgumentNullException(nameof(settingsOptions));
            }

            _settings = settingsOptions.Value;
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
        }

        public async Task<CommandResult> ReadCommandAsync()
        {
            var client = _clientFactory.CreateClient();
            var message = new HttpRequestMessage(
                HttpMethod.Get,
                _settings.CommandUrl);
            
            var response = await client.SendAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Status code is not OK: {response.StatusCode}");
                return CommandResult.CreateTryAgainResult();
            }

            var result = (await response.Content.ReadAsStringAsync()).Trim();
            if (string.IsNullOrEmpty(result))
            {
                return CommandResult.CreateNoCommandResult();
            }

            if (!byte.TryParse(result, out byte command))
            {
                _logger.LogWarning($"Cant parse command: {result}");
                return CommandResult.CreateTryAgainResult();
            }  
            
            return new CommandResult(CommandReadingStatus.CommandFound, command);
        } 

        private readonly SiteCommandReaderSettings _settings;

        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;
    }
}