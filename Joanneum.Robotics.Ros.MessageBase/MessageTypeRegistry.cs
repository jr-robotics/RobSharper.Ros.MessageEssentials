using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class MessageTypeRegistry
    {
        private readonly IDictionary<Type, IMessageTypeInfo> _messageTypes = new Dictionary<Type, IMessageTypeInfo>();
        private readonly IDictionary<string, IMessageTypeInfo> _rosTypes = new Dictionary<string, IMessageTypeInfo>();
        
        public IMessageTypeInfo GetOrCreateMessageTypeInfo(object rosMessage)
        {
            if (rosMessage == null) throw new ArgumentNullException(nameof(rosMessage));

            return GetOrCreateMessageTypeInfo(rosMessage.GetType());
        }

        public IMessageTypeInfo GetOrCreateMessageTypeInfo(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (_messageTypes.ContainsKey(type))
                return _messageTypes[type];

            
            MessageTypeInfo messageInfo;
            if (RosMessageDescriptorFactory.CanCreate(type))
            {
                var messageDescriptor = RosMessageDescriptorFactory.Create(type);
                
                messageInfo = CreateMessageTypeInfo(messageDescriptor);
            }
            else
            {
                throw new NotSupportedException();
            }
            
            _messageTypes.Add(type, messageInfo);
            _rosTypes.Add(messageInfo.Type.ToString(), messageInfo);
            
            return messageInfo;
        }

        private MessageTypeInfo CreateMessageTypeInfo(RosMessageDescriptor messageDescriptor)
        {
            var md5 = CalculateMd5Sum(messageDescriptor);
            
            return new MessageTypeInfo(messageDescriptor, md5);
        }

        private string CalculateMd5Sum(RosMessageDescriptor messageDescriptor)
        {
            var firstElement = true;
            var md5 = MD5.Create();

            using (var ms = new MemoryStream())
            {
                var writer = new StreamWriter(ms, Encoding.ASCII)
                {
                    AutoFlush = true
                };

                // TODO MD5 of Constants

                // MD5 Of Fields
                foreach (var field in messageDescriptor.Fields)
                {
                    if (firstElement)
                    {
                        firstElement = false;
                    }
                    else
                    {
                        writer.Write("\n");
                    }

                    if (field.RosType.IsBuiltIn)
                    {
                        writer.Write(field.RosType);
                    }
                    else
                    {
                        Type mappedFieldType;

                        if (field.RosType.IsArray)
                        {
                            mappedFieldType = field.RosType.MappedType
                                .GetInterfaces()
                                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                                .Select(t => t.GetGenericArguments()[0])
                                .FirstOrDefault();
                        }
                        else
                        {
                            mappedFieldType = field.RosType.MappedType;
                        }
                        
                        var typeInfo = GetOrCreateMessageTypeInfo(mappedFieldType);
                        var typeHash = typeInfo.MD5Sum;

                        writer.Write(typeHash);
                    }
                    
                    writer.Write(" ");
                    writer.Write(field.RosIdentifier);
                }
                
                ms.Position = 0;
                var md5Bytes = md5.ComputeHash(ms);
                var md5String = ToHexString(md5Bytes);

                return md5String;
            }
        }

        private static string ToHexString(byte[] buffer)
        {
            var sb = new StringBuilder(buffer.Length * 2);
            foreach (byte b in buffer)
            {
                sb.AppendFormat("{0:x2}", b);
            }

            return sb.ToString();
        }
    }
}