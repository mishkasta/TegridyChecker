using System.Security.Permissions;
using Microsoft.Extensions.Logging;
using TegridyChecker.Foundation.Interfaces;

namespace TegridyChecker.Foundation.Implementations
{
    public class NetFrameworkCodeExecutor : INetFrameworkCodeExecutor
    {
        private readonly ILogger<NetFrameworkCodeExecutor> _logger;


        public NetFrameworkCodeExecutor(ILogger<NetFrameworkCodeExecutor> logger)
        {
            _logger = logger;
        }


        public void ExecuteSomethingFromNetFrameworkLibrary()
        {
            var uselessStuff = new DataProtectionPermission(PermissionState.None);

            var someStuff = uselessStuff.Flags;

            _logger.LogInformation("Use some stuff from .NET Library: {someStuff}", someStuff);
        }
    }
}