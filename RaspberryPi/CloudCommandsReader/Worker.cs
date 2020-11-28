using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CloudCommandsReader
{
    public class Worker : BackgroundService
    {
        public Worker(
            ILogger<Worker> logger,
            CloudStorageCommandReader commandReader,
            CommandSender sender)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _commandReader = commandReader ?? throw new ArgumentNullException(nameof(commandReader));
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    var commandResult = await _commandReader.ReadCommandAsync();
                    switch (commandResult.Status)
                    {
                        case CommandReadingStatus.Retry:
                            continue;
                        
                        case CommandReadingStatus.CommandFound:
                            await _sender.SendAsync(commandResult.Command);
                            break;
                        
                        case CommandReadingStatus.NoCommand:
                            await Task.Delay(1000, stoppingToken);
                            break;
                    }

                    _logger.LogInformation($"Command result: {commandResult.Status} {commandResult.Command}");                    
                }
                catch (TaskCanceledException)
                {
                    _logger.LogInformation("Stopping worker.");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error occured");
                    await Task.Delay(5000);
                }
            }
        }

        private readonly ILogger<Worker> _logger;

        private readonly CloudStorageCommandReader _commandReader;

        private readonly CommandSender _sender;
    }
}
