using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using TegridyChecker.Interfaces;

namespace TegridyChecker.Commands
{
    public class HelpCommand : CommandBase
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ICommandFactory _commandFactory;


        public override string Description => "Nu ty ponel";

        public override string Format => $"'{Command} [<command-name>]'";


        public HelpCommand(
            IHostEnvironment hostEnvironment,
            ICommandFactory commandFactory,
            string command)
            : base(command)
        {
            _hostEnvironment = hostEnvironment;
            _commandFactory = commandFactory;
        }


        protected override Task ExecuteAsync(IReadOnlyCollection<string> args, CancellationToken cancellationToken)
        {
            if (args.Count > 0)
            {
                foreach (var command in args.Distinct())
                {
                    PrintCommand(command);
                }
            }
            else
            {
                PrintAllCommands();
            }

            return Task.CompletedTask;
        }


        private void PrintAllCommands()
        {
            var sb = new StringBuilder()
                .AppendLine(
                    $"{_hostEnvironment.ApplicationName} - very important app developed for being very useless. It's just for some very important (but still unknown) purposes.")
                .AppendLine($"It's run on {_hostEnvironment.EnvironmentName} environment.");

            sb.AppendLine("Supported commands:");

            var commands = _commandFactory.CreateAll();
            foreach (var command in commands)
            {
                sb.AppendLine($"    {command.Command} - {command.Description}");
            }

            Console.WriteLine(sb);
        }

        private void PrintCommand(string commandName)
        {
            var command = _commandFactory.GetOrCreateFor(commandName);
            if (command == null)
            {
                Console.WriteLine($"Command {commandName} is unknown");

                return;
            }

            var sb = new StringBuilder()
                .AppendLine($"Description: {command.Description} {command.DetailedDescription}")
                .AppendLine($"Format: {command.Format}");

            if (command.SupportedArgs.Count != 0)
            {
                sb.AppendLine("Supported args:");

                foreach (var supportedArg in command.SupportedArgs)
                {
                    sb.AppendLine($"    {supportedArg}");
                }
            }

            Console.WriteLine(sb);
        }
    }
}