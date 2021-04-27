using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TegridyChecker.Commands
{
    public class CheckPrivelegesCommand : CommandBase
    {
        public override string Description => "Check your priveleges. Do you have some priveleges???";


        public CheckPrivelegesCommand(string command)
            : base(command)
        {

        }


        protected override bool SetupAndValidateArgs(IReadOnlyCollection<string> args)
        {
            throw new System.NotImplementedException();
        }

        protected override Task ExecuteAsync(IReadOnlyCollection<string> args, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}