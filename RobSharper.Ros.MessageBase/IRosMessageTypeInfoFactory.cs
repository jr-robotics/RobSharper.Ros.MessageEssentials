using System;

namespace RobSharper.Ros.MessageBase
{
    public interface IRosMessageTypeInfoFactory
    {
        bool CanCreate(Type messageType);
        IRosMessageTypeInfo Create(Type messageType);
    }
}