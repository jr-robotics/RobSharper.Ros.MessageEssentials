using System;
using RobSharper.Ros.MessageBase;

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
                    _typeInfo = MessageTypeRegistry.Instance
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
}