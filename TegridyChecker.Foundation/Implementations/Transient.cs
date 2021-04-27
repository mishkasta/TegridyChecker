using System;
using TegridyChecker.Foundation.Interfaces;

namespace TegridyChecker.Foundation.Implementations
{
    public class Transient : IAmTransient
    {
        private Guid Guid { get; }


        public Transient()
        {
            Guid = Guid.NewGuid();
        }


        public Guid GetGuid() => Guid;
    }
}