using Microsoft.Extensions.Logging;
using TegridyChecker.Foundation.Interfaces;

namespace TegridyChecker.Foundation.Implementations
{
    public class SomeTransientProveryator : TransientProveryatorBase
    {
        public SomeTransientProveryator(IAmTransient transient, ILogger<SomeTransientProveryator> logger)
            : base(transient, logger)
        {

        }
    }
}