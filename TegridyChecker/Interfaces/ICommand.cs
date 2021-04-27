using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TegridyChecker.Interfaces
{
    public interface ICommand
    {
        string Command { get; }

        string Description { get; }

        string DetailedDescription { get; }

        string Format { get; }

        IReadOnlyCollection<string> SupportedArgs { get; }


        Task ExecuteAsync(CancellationToken cancellationToken, params string[] args);
    }
}