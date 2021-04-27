using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TegridyChecker.Interfaces;

namespace TegridyChecker.Commands
{
    public abstract class CommandBase : ICommand
    {
        public string Command { get; }

        public virtual string Format => $"'{Command}'";

        public abstract string Description { get; }

        public virtual string DetailedDescription { get; }

        public virtual IReadOnlyCollection<string> SupportedArgs => Array.Empty<string>();


        protected CommandBase(string command)
        {
            Command = command;
        }


        public Task ExecuteAsync(CancellationToken cancellationToken, params string[] args)
        {
            var areArgsValid = SetupAndValidateArgs(args);
            if (areArgsValid)
            {
                return ExecuteAsync(args, cancellationToken);
            }

            Console.WriteLine($"Command format: {Format}");

            return Task.CompletedTask;
        }


        protected virtual bool SetupAndValidateArgs(IReadOnlyCollection<string> args) => true;

        protected abstract Task ExecuteAsync(IReadOnlyCollection<string> args, CancellationToken cancellationToken);
    }
}