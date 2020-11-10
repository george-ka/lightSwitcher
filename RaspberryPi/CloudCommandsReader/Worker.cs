using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CloudCommandsReader
{
    public class Worker : BackgroundService
    {
        public Worker(
            ILogger<Worker> logger,
            CloudStorageCommandReader commandReader)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _commandReader = commandReader ?? throw new ArgumentNullException(nameof(commandReader));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    var commandResult = await _commandReader.ReadCommandAsync();
                    _logger.LogInformation($"Command result: {commandResult.Status} {commandResult.Command}");
                    
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured");
            }
        }

        private readonly ILogger<Worker> _logger;

        private readonly CloudStorageCommandReader _commandReader;
    }
}
