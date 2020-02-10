using System;

namespace RobSharper.Ros.MessageBase
{
    public interface IRosMessageTypeInfoFactory
    {
        bool CanCreate(Type messageType);
        DescriptorBasedMessageTypeInfo Create(Type messageType);
    }
}