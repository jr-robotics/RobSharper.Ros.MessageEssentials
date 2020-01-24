using System;
using System.Collections.Generic;
using System.Linq;

namespace RobSharper.Ros.MessageBase
{
    public class MessageTypeRegistry
    {
        private static MessageTypeRegistry _instance;
        public static MessageTypeRegistry Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(MessageTypeRegistry))
                    {
                        if (_instance == null)
                        {
                            _instance = new MessageTypeRegistry();
                        }
                    }
                }

                return _instance;
            }
            
            set => _instance = value;
        }

        private readonly IDictionary<Type, IMessageTypeInfo> _messageTypes = new Dictionary<Type, IMessageTypeInfo>();
        private readonly IDictionary<string, IMessageTypeInfo> _rosTypes = new Dictionary<string, IMessageTypeInfo>();

        public IMessageTypeInfo this[Type mappedType]
        {
            get => _messageTypes[mappedType];
        }

        public IMessageTypeInfo this[string rosTypeName]
        {
            get => _rosTypes[rosTypeName];
        }

        public IMessageTypeInfo GetOrCreateMessageTypeInfo(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (_messageTypes.ContainsKey(type))
                return _messageTypes[type];

            
            RosMessageDescriptor messageDescriptor;
            if (AttributeBasedRosMessageDescriptorFactory.CanCreate(type))
            {
                messageDescriptor = AttributeBasedRosMessageDescriptorFactory.Create(type);
            }
            else
            {
                throw new NotSupportedException();
            }
            
            var messageInfo = CreateMessageTypeInfo(messageDescriptor);
            _messageTypes.Add(type, messageInfo);
            _rosTypes.Add(messageInfo.Type.ToString(), messageInfo);
            
            return messageInfo;
        }

        private MessageTypeInfo CreateMessageTypeInfo(RosMessageDescriptor messageDescriptor)
        {
            var dependencies = new List<IMessageTypeInfo>();
            foreach (var dependentField in messageDescriptor.Fields)
            {
                if (dependentField.RosType.IsBuiltIn)
                    continue;
                
                Type mappedFieldType;

                if (dependentField.RosType.IsArray)
                {
                    mappedFieldType = dependentField
                        .MappedProperty
                        .PropertyType
                        .GetInterfaces()
                        .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        .Select(t => t.GetGenericArguments()[0])
                        .FirstOrDefault();
                }
                else
                {
                    mappedFieldType = dependentField
                        .MappedProperty
                        .PropertyType;
                }
                
                var dependency = GetOrCreateMessageTypeInfo(mappedFieldType);
                dependencies.Add(dependency);
            }

            return new MessageTypeInfo(messageDescriptor, dependencies);
        }
    }
}