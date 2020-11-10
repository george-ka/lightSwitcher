using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace CloudCommandsReader
{
    public class CloudStorageCommandReader
    {
        public CloudStorageCommandReader(
            IOptions<CloudCommandsReaderSettings> settingsOptions,
            ILogger<CloudStorageCommandReader> logger)
        {
            if (settingsOptions == null || settingsOptions.Value == null)
            {
                throw new ArgumentNullException(nameof(settingsOptions));
            }

            var settings = settingsOptions.Value;
            
            if (!string.IsNullOrWhiteSpace(settings.KeyFilePath) && File.Exists(settings.KeyFilePath))
            {
                var credentials = GoogleCredential.FromFile(settings.KeyFilePath);
                _storageClient = StorageClient.Create(credentials);
            }
            else
            {
                _storageClient = StorageClient.Create();
            }

            _bucketName = settings.BucketName;
            _commandFileNamePrefix = settings.CommandFileNamePrefix;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
        }
        public async Task<CommandResult> ReadCommandAsync()
        {
            var pagesEnumerable = _storageClient.ListObjectsAsync(_bucketName, _commandFileNamePrefix);
            var page = await pagesEnumerable.ReadPageAsync(10);
            var commandObjectReference = page.FirstOrDefault();
            if (commandObjectReference == null)
            {
                return CommandResult.CreateNoCommandResult();
            }
                
            try
            {
                byte result;
                using (var stream = new MemoryStream())
                using (var reader = new StreamReader(stream))
                {
                    await _storageClient.DownloadObjectAsync(
                        _bucketName, 
                        commandObjectReference.Name, 
                        stream,
                        new DownloadObjectOptions
                        {
                            IfGenerationMatch = commandObjectReference.Generation
                        });

                    stream.Position = 0;
                    var command = reader.ReadToEnd();
                    _logger.LogInformation($"Found a command '{command}'");
                    result = byte.Parse(command);
                }
            
                await _storageClient.DeleteObjectAsync(
                    _bucketName, 
                    commandObjectReference.Name, 
                    new DeleteObjectOptions
                    {
                        IfGenerationMatch = commandObjectReference.Generation
                    });

                _logger.LogInformation("Object deleted");

                return new CommandResult(CommandReadingStatus.CommandFound, result);
            }
            catch (Google.GoogleApiException e)
            {
                if (e.HttpStatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                    _logger.LogInformation("Can't delete: new version available");
                    return CommandResult.CreateTryAgainResult();
                }
                
                throw;
            }
        } 

        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        private readonly string _commandFileNamePrefix;

        private readonly ILogger _logger;
    }
}