using System;

namespace RobSharper.Ros.MessageEssentials
{
    public interface IRosMessageTypeInfoFactory
    {
        bool CanCreate(Type messageType);
        IRosMessageTypeInfo Create(Type messageType);
    }
}