using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TegridyChecker.Commands;
using TegridyChecker.Foundation.Implementations;
using TegridyChecker.Foundation.Interfaces;
using TegridyChecker.Interfaces;

namespace TegridyChecker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var envs = Environment.GetEnvironmentVariables().OfType<DictionaryEntry>().OrderBy(e => e.Key.ToString()).ToList();
            foreach (DictionaryEntry dictionaryEntry in envs)
            {
                Console.WriteLine($"{dictionaryEntry.Key} = {dictionaryEntry.Value}");
            }

            using var host = CreateHostBuilder(args)
                .UseConsoleLifetime()
                .Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            try
            {
                host.Start();

                host.WaitForShutdown();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Closing program due to unhandled exception");

                try
                {
                    host.StopAsync().Wait();
                }
                catch (AggregateException stopException)
                {
                    if (!(stopException.InnerException is TaskCanceledException))
                    {
                        logger.LogError(stopException, "Failed to shut down application smoothly");
                    }
                }
            }

            logger.LogInformation("Application stopped");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    // builder.AddJsonFile("mysettings.json", true, false);
                    builder.AddXmlFile("xmlsettings.xml");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    services.Configure<TegridyCheckerOptions>(hostContext.Configuration.GetSection("TegridyOptions"));
                    services.Configure<TegridyCheckerOptions>(options =>
                    {
                        // options.ShutdownDelay = TimeSpan.Zero;
                    });

                    services.Configure<HostOptions>(options =>
                    {
                        options.ShutdownTimeout = TimeSpan.FromSeconds(5);
                    });

                    services.AddScoped<ICommandFactory, CommandFactory>();

                    services.AddSingleton<INetFrameworkCodeExecutor, NetFrameworkCodeExecutor>();

                    services
                        .AddScoped<SomeTransientProveryator>()
                        .AddScoped<AnotherTransientProveryator>()
                        .AddTransient<IAmTransient, Transient>();
                });
    }
}
