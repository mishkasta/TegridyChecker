using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TegridyChecker.Interfaces;

namespace TegridyChecker
{
    public sealed class Worker : IHostedService, IDisposable
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<Worker> _logger;
        private readonly TegridyCheckerOptions _options;

        private readonly CancellationTokenSource _cancellationTokenSource;


        public Worker(
            IHostApplicationLifetime applicationLifetime,
            IServiceScopeFactory scopeFactory,
            ILogger<Worker> logger,
            IOptions<TegridyCheckerOptions> options)
        {
            _applicationLifetime = applicationLifetime;
            _scopeFactory = scopeFactory;
            _logger = logger;
            _options = options.Value;

            _cancellationTokenSource = new CancellationTokenSource();
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var delay = _options.StartDelay;
            if (delay != TimeSpan.Zero)
            {
                _logger.LogInformation($"Starting with delay {delay}");

                await Task.Delay(delay, cancellationToken);
            }

            _applicationLifetime.ApplicationStarted.Register(OnApplicationStarted);

            _logger.LogInformation("Service started");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var delay = _options.ShutdownDelay;
            if (delay != TimeSpan.Zero)
            {
                _logger.LogInformation($"Stopping service with delay {delay}");

                await Task.Delay(delay);
            }

            _cancellationTokenSource.Cancel();

            _logger.LogInformation("Service stopped");
        }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
        }


        private async void OnApplicationStarted()
        {
            await Task.Delay(1_000);

            Console.WriteLine(Ascii.Intro);

            try
            {
                await Task.Run(ListenForCommandsAsync);
            }
            catch (OperationCanceledException)
            {
                _logger.LogDebug("Listening cancelled");
            }
        }

        private async Task ListenForCommandsAsync()
        {
            var cancellationToken = _cancellationTokenSource.Token;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                Console.Write("> ");

                cancellationToken.ThrowIfCancellationRequested();

                var inputString = Console.ReadLine();

                if (string.IsNullOrEmpty(inputString))
                {
                    continue;
                }

                _logger.LogInformation("Received input: {input}", inputString);

                var input = inputString
                    .ToLowerInvariant()
                    .Split(" ");

                cancellationToken.ThrowIfCancellationRequested();

                using var scope = _scopeFactory.CreateScope();

                var commandFactory = scope.ServiceProvider.GetRequiredService<ICommandFactory>();

                var commandName = input.First();
                var command = commandFactory.GetOrCreateFor(commandName);

                if (command == null)
                {
                    Console.WriteLine("Unknown command. Use 'help' command in order to display available commands");

                    continue;
                }

                var args = input.Skip(1).ToArray();
                await command.ExecuteAsync(cancellationToken, args);
            }
        }
    }
}
