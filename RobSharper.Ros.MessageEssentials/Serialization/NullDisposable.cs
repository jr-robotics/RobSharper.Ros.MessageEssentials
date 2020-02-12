using System;

namespace RobSharper.Ros.MessageEssentials.Serialization
{
    internal class NullDisposable : IDisposable
    {
        public static readonly NullDisposable Instance = new NullDisposable();
        
        public void Dispose()
        {
        }
    }
}