using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TegridyChecker.Foundation.Implementations;

namespace TegridyChecker.Commands
{
    public class CheckTransientsCommand : CommandBase
    {
        private readonly SomeTransientProveryator _someTransientProveryator;
        private readonly AnotherTransientProveryator _anotherTransientProveryator;


        public override string Description => "Makes sure that transients are transients.";


        public CheckTransientsCommand(
            SomeTransientProveryator someTransientProveryator,
            AnotherTransientProveryator anotherTransientProveryator,
            string command)
            : base(command)
        {
            _someTransientProveryator = someTransientProveryator;
            _anotherTransientProveryator = anotherTransientProveryator;
        }


        protected override Task ExecuteAsync(IReadOnlyCollection<string> args, CancellationToken cancellationToken)
        {
            _someTransientProveryator.DoSomethingWithTransient();
            _anotherTransientProveryator.DoSomethingWithTransient();

            Console.WriteLine("Checkni logi");

            return Task.CompletedTask;
        }
    }
}