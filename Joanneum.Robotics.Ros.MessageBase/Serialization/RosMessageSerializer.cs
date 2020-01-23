using System;
using System.Collections.Generic;
using System.IO;

namespace Joanneum.Robotics.Ros.MessageBase.Serialization
{
    public class RosMessageSerializer
    {
        public static RosMessageSerializer Instance { get; set; } = new RosMessageSerializer(MessageBase.MessageTypeRegistry.Instance);
        
        public IList<IRosMessageFormatter> MessageFormatters { get; } = new List<IRosMessageFormatter>
        {
            new RosMessageFormatter() // Default formatter
        };
        
        public MessageTypeRegistry MessageTypeRegistry { get; }
        
        private RosMessageSerializer(MessageTypeRegistry messageTypeRegistry)
        {
            MessageTypeRegistry = messageTypeRegistry ?? throw new ArgumentNullException(nameof(messageTypeRegistry));
        }
        
        public TMessage Deserialize<TMessage>(Stream input)
        {
            return (TMessage) Deserialize(typeof(TMessage), input);
        }

        public object Deserialize(Type messageType, Stream input)
        {
            if (messageType == null) throw new ArgumentNullException(nameof(messageType));
            if (input == null) throw new ArgumentNullException(nameof(input));
            
            var messageTypeInfo = MessageTypeRegistry.GetOrCreateMessageTypeInfo(messageType);
            var formatter = GetFormatter(messageTypeInfo);
            var context = new SerializationContext(input, MessageFormatters, MessageTypeRegistry);

            return formatter.Deserialize(context, messageTypeInfo);
        }

        public void Serialize(object message, Stream output)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (output == null) throw new ArgumentNullException(nameof(output));

            var messageTypeInfo = MessageTypeRegistry.GetOrCreateMessageTypeInfo(message.GetType());
            var formatter = GetFormatter(messageTypeInfo);
            var context = new SerializationContext(output, MessageFormatters, MessageTypeRegistry);
            
            formatter.Serialize(context, messageTypeInfo, message);
        }

        private IRosMessageFormatter GetFormatter(IMessageTypeInfo messageTypeInfo)
        {
            var formatter = MessageFormatters.FindFormatterFor(messageTypeInfo);

            if (formatter == null)
                throw new NotSupportedException($"No formatter for message {messageTypeInfo} found.");

            return formatter;
        }
    }
}