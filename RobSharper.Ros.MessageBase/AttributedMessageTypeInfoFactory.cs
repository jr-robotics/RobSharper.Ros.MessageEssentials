using System;

namespace RobSharper.Ros.MessageBase
{
    public class AttributedMessageTypeInfoFactory : IRosMessageTypeInfoFactory
    {
        private readonly MessageTypeRegistry _typeRegistry;

        public AttributedMessageTypeInfoFactory(MessageTypeRegistry typeRegistry)
        {
            _typeRegistry = typeRegistry ?? throw new ArgumentNullException(nameof(typeRegistry));
        }

        public bool CanCreate(Type messageType)
        {
            return AttributeBasedRosMessageDescriptorFactory.CanCreate(messageType);
        }

        public IRosMessageTypeInfo Create(Type messageType)
        {
            var descriptor = AttributeBasedRosMessageDescriptorFactory.Create(messageType);
            var messageInfo = DescriptorBasedMessageTypeInfo.Create(messageType, descriptor, _typeRegistry);

            return messageInfo;
        }
    }
}