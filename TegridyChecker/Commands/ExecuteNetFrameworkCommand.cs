using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TegridyChecker.Foundation.Interfaces;

namespace TegridyChecker.Commands
{
    public class ExecuteNetFrameworkCommand : CommandBase
    {
        private readonly INetFrameworkCodeExecutor _netFrameworkCodeExecutor;


        public override string Description => "Executes code from library came .NET Framework.";


        public ExecuteNetFrameworkCommand(
            INetFrameworkCodeExecutor netFrameworkCodeExecutor,
            string command)
            : base(command)
        {
            _netFrameworkCodeExecutor = netFrameworkCodeExecutor;
        }


        protected override Task ExecuteAsync(IReadOnlyCollection<string> args, CancellationToken cancellationToken)
        {
            _netFrameworkCodeExecutor.ExecuteSomethingFromNetFrameworkLibrary();

            return Task.CompletedTask;
        }
    }
}