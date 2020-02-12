using System;

namespace RobSharper.Ros.MessageEssentials
{
    public class RosMessageTypeInfoCreationException : Exception
    {
        public Type MessageType { get; }
        public Type FactoryType { get; }
        
        public RosMessageTypeInfoCreationException() { }

        public RosMessageTypeInfoCreationException(string message) : base(message)
        {
            
        }

        public RosMessageTypeInfoCreationException(string message, Exception innerException) : base(message, innerException)
        {
            
        }

        public RosMessageTypeInfoCreationException(string message, Type messageType, Type factoryType, Exception innerException) : base(message, innerException)
        {
            MessageType = messageType;
            FactoryType = factoryType;
        }
    }
}