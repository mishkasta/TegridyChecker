using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TegridyChecker.Foundation.Implementations;
using TegridyChecker.Foundation.Interfaces;
using TegridyChecker.Interfaces;

namespace TegridyChecker.Commands
{
    public class CommandFactory : ICommandFactory
    {
        private const string CheckPriveleges = "check-priveleges";
        private const string HelpCommand = "help";
        private const string ExitCommand = "exit";
        private const string ExecuteNetFrameworkCodeCommand = "execute-net-framework-code";
        private const string CheckTransientCommand = "check-transient";

        private readonly IReadOnlyDictionary<string, Func<ICommand>> _commandCreators;
        private readonly IDictionary<string, ICommand> _cachedCommands;


        public CommandFactory(
            IHostEnvironment hostEnvironment,
            IHostApplicationLifetime hostLifetime,
            IOptions<TegridyCheckerOptions> options,
            INetFrameworkCodeExecutor netFrameworkCodeExecutor,
            SomeTransientProveryator someTransientProveryator,
            AnotherTransientProveryator anotherTransientProveryator)
        {
            _commandCreators = new Dictionary<string, Func<ICommand>>
            {
                {HelpCommand, () => new HelpCommand(hostEnvironment, this, HelpCommand)},
                {CheckPriveleges, () => new CheckPrivelegesCommand(CheckPriveleges)},
                {CheckTransientCommand, () => new CheckTransientsCommand(someTransientProveryator, anotherTransientProveryator, CheckTransientCommand)},
                {ExecuteNetFrameworkCodeCommand, () => new ExecuteNetFrameworkCommand(netFrameworkCodeExecutor, ExecuteNetFrameworkCodeCommand)},
                {ExitCommand, () => new ExitCommand(hostLifetime, options, ExitCommand)}
            };
            _cachedCommands = new Dictionary<string, ICommand>();
        }


        public ICommand GetOrCreateFor(string commandName)
        {
            if (_cachedCommands.TryGetValue(commandName.ToLowerInvariant(), out var command))
            {
                return command;
            }

            if (!_commandCreators.TryGetValue(commandName, out var creator))
            {
                return null;
            }

            command = creator();
            _cachedCommands.Add(commandName, command);

            return command;
        }

        public IReadOnlyCollection<ICommand> CreateAll()
        {
            if (_cachedCommands.Count == _commandCreators.Count)
            {
                return _cachedCommands.Values.ToList();
            }

            var commands = _commandCreators
                .Where(kvp => !_cachedCommands.ContainsKey(kvp.Key))
                .Select(kvp => new { kvp.Key, Command = kvp.Value() })
                .ToList();

            foreach (var command in commands)
            {
                _cachedCommands.Add(command.Key, command.Command);
            }

            return _cachedCommands.Values.ToList();
        }
    }
}