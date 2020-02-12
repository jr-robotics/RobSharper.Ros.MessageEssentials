using System;
using System.Collections.Generic;
using System.IO;

namespace RobSharper.Ros.MessageEssentials.Serialization
{
    public class SerializationContext
    {
        public SerializationContext(Stream stream, IEnumerable<IRosMessageFormatter> messageFormatters,
            MessageTypeRegistry messageTypeRegistry)
        {
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            MessageFormatters = messageFormatters ?? throw new ArgumentNullException(nameof(messageFormatters));
            MessageTypeRegistry = messageTypeRegistry ?? throw new ArgumentNullException(nameof(messageTypeRegistry));
        }

        public Stream Stream { get; }
        public IEnumerable<IRosMessageFormatter> MessageFormatters { get; }
        public MessageTypeRegistry MessageTypeRegistry { get; }
    }
}