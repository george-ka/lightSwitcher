using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace CloudCommandsReader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var section = hostContext.Configuration.GetSection("CloudCommandsReaderSettings");
                    services
                        .Configure<CloudCommandsReaderSettings>(hostContext.Configuration.GetSection("CloudCommandsReaderSettings"))
                        .Configure<CommandSenderSettings>(hostContext.Configuration.GetSection("CommandSenderSettings"))
                        .AddSingleton<CloudStorageCommandReader>()
                        .AddSingleton<CommandSender>()
                        .AddHttpClient()
                        .AddHostedService<Worker>();
                });
    }
}
