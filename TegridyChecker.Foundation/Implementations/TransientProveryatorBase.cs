using Microsoft.Extensions.Logging;
using TegridyChecker.Foundation.Interfaces;

namespace TegridyChecker.Foundation.Implementations
{
    public abstract class TransientProveryatorBase : ITransientProveryator
    {
        private readonly IAmTransient _transient;
        private readonly ILogger _logger;


        protected TransientProveryatorBase(IAmTransient transient, ILogger logger)
        {
            _transient = transient;
            _logger = logger;
        }


        public void DoSomethingWithTransient()
        {
            _logger.LogInformation("Proveryator proveril transient class and there is a conclusion: it's transient for sure. Look at this guid: {guid}", _transient.GetGuid());
        }
    }
}