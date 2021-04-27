using System.Collections.Generic;

namespace TegridyChecker.Interfaces
{
    public interface ICommandFactory
    {
        ICommand GetOrCreateFor(string command);

        IReadOnlyCollection<ICommand> CreateAll();
    }
}