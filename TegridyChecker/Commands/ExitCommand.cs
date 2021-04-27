using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace TegridyChecker.Commands
{
    public class ExitCommand : CommandBase
    {
        private const string DelayArgShortName = "-d";
        private const string DelayArgName = "--delay";

        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly TegridyCheckerOptions _options;


        public override string Format => $"'{Command} [{DelayArgShortName}|{DelayArgName} <NN[s|m|h]>]'";

        public override string Description => "Closes the application.";

        public override string DetailedDescription =>
            $"If '{DelayArgShortName}'/'{DelayArgName}' is specified, application will be closed after delay specified in seconds (i.e. 42s), minutes (42m), hours (42h)";


        public ExitCommand(
            IHostApplicationLifetime hostApplicationLifetime,
            IOptions<TegridyCheckerOptions> options,
            string command)
            : base(command)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _options = options.Value;
        }


        protected override bool SetupAndValidateArgs(IReadOnlyCollection<string> args)
        {
            if (args.Count == 0)
            {
                return true;
            }

            if (args.Count != 2)
            {
                return false;
            }

            var argName = args.First();
            var argValue = args.Last();

            if (argName != DelayArgName && argName != DelayArgShortName)
            {
                return false;
            }

            var isValueSpecified = argValue.Length > 1;
            if (!isValueSpecified)
            {
                return false;
            }

            var delayValue = argValue.Substring(0, argValue.Length - 1);
            var timeUnit = argValue.Substring(argValue.Length - 1, 1);

            if (!TryGetDelay(delayValue, timeUnit, out var delay))
            {
                return false;
            }

            _options.ShutdownDelay = delay;

            return true;
        }

        protected override Task ExecuteAsync(IReadOnlyCollection<string> args, CancellationToken cancellationToken)
        {
            _hostApplicationLifetime.StopApplication();

            return Task.CompletedTask;
        }


        private bool TryGetDelay(string delayValue, string delayTimeUnits, out TimeSpan timeSpan)
        {
            timeSpan = TimeSpan.Zero;

            if (!double.TryParse(delayValue, out var value))
            {
                return false;
            }

            switch (delayTimeUnits)
            {
                case "s":
                    timeSpan = TimeSpan.FromSeconds(value);
                    break;
                case "m":
                    timeSpan = TimeSpan.FromMinutes(value);
                    break;
                case "h":
                    timeSpan = TimeSpan.FromHours(value);
                    break;
                default:
                    return false;
            }

            return true;
        }
    }
}