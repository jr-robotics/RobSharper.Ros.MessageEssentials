using System;
using RobSharper.Ros.MessageBase;
using Uml.Robotics.Ros;
using MessageTypeRegistry = RobSharper.Ros.MessageBase.MessageTypeRegistry;

namespace RobSharper.Ros.Adapters.UmlRobotics
{
    public class MessageTypeInfoProvider
    {
        private readonly Type _messageType;
        private IMessageTypeInfo _typeInfo;

        public IMessageTypeInfo TypeInfo
        {
            get
            {
                if (_typeInfo == null)
                {
                    _typeInfo = RobSharperInfrastructure.MessageTypeRegistry
                        .GetOrCreateMessageTypeInfo(_messageType);
                }

                return _typeInfo;
            }
        }

        public MessageTypeInfoProvider(Type messageType)
        {
            _messageType = messageType;
        }
    }
    
    public class MessageTypeInfoProvider<TMessage> : MessageTypeInfoProvider
        where TMessage : RosMessage
    {
        public MessageTypeInfoProvider() : base(typeof(TMessage))
        {
        }
    }
}