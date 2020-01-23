using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Joanneum.Robotics.Ros.MessageBase.Serialization
{
    public static class RosSerializationExtensions
    {
        public static byte[] ToLittleEndian(this byte[] source)
        {
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(source);
            
            return source;
        }

        public static void FromLittleEndian(this byte[] source)
        {
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(source);
        }

        public static void Write(this Stream s, byte[] buffer)
        {
            s.Write(buffer, 0, buffer.Length);
        }

        public static void Read(this Stream s, byte[] buffer)
        {
            s.Read(buffer, 0, buffer.Length);
        }

        public static byte[] ReadBytes(this Stream s, int numBytes)
        {
            var buffer = new byte[numBytes];
            s.Read(buffer);

            return buffer;
        }

        public static IRosMessageFormatter FindFormatterFor(this IEnumerable<IRosMessageFormatter> formatters, IMessageTypeInfo messageTypeInfo)
        {
            return formatters.FirstOrDefault(x => x.CanSerialize(messageTypeInfo));
        }
    }
}