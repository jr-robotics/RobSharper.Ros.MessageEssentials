using System;
using RobSharper.Ros.MessageBase;
using Uml.Robotics.Ros;

namespace RobSharper.Ros.Adapters.UmlRobotics
{
    public class DescriptorBasedMessageTypeInfoProvider
    {
        private readonly Type _messageType;
        private DescriptorBasedMessageTypeInfo _typeInfo;

        public DescriptorBasedMessageTypeInfo TypeInfo
        {
            get
            {
                if (_typeInfo == null)
                {
                    var messageTypeInfo = RobSharperInfrastructure
                        .MessageTypeRegistry
                        .GetOrCreateMessageTypeInfo(_messageType);

                    _typeInfo = (DescriptorBasedMessageTypeInfo) messageTypeInfo;
                }

                return _typeInfo;
            }
        }

        public DescriptorBasedMessageTypeInfoProvider(Type messageType)
        {
            _messageType = messageType;
        }
    }
    
    public class DescriptorBasedMessageTypeInfoProvider<TMessage> : DescriptorBasedMessageTypeInfoProvider
        where TMessage : RosMessage
    {
        public DescriptorBasedMessageTypeInfoProvider() : base(typeof(TMessage))
        {
        }
    }
}