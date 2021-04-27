using Microsoft.Extensions.Logging;
using TegridyChecker.Foundation.Interfaces;

namespace TegridyChecker.Foundation.Implementations
{
    public class AnotherTransientProveryator : TransientProveryatorBase
    {
        public AnotherTransientProveryator(IAmTransient transient, ILogger<AnotherTransientProveryator> logger)
            : base(transient, logger)
        {

        }
    }
}