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
                    var commandReaderType = hostContext.Configuration.GetValue<string>("CommandReaderType");
                    Console.WriteLine(commandReaderType);
                    if (commandReaderType == "SiteCommandsReader")
                    {
                        services
                            .Configure<SiteCommandReaderSettings>(hostContext.Configuration.GetSection("SiteCommandsReaderSettings"))
                            .AddSingleton<ICommandReader, SiteCommandReader>();
                    }
                    else
                    {
                        services
                            .Configure<CloudCommandsReaderSettings>(hostContext.Configuration.GetSection("CloudCommandsReaderSettings"))
                            .AddSingleton<ICommandReader, CloudStorageCommandReader>();
                    }

                    services
                        .Configure<CommandSenderSettings>(hostContext.Configuration.GetSection("CommandSenderSettings"))
                        .AddSingleton<CommandSender>()
                        .AddHttpClient()
                        .AddHostedService<Worker>();
                });
    }
}
