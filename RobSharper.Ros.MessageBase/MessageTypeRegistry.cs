using System;
using System.Collections.Generic;
using System.Linq;

namespace RobSharper.Ros.MessageBase
{
    public class MessageTypeRegistry
    {
        private readonly IDictionary<Type, IRosMessageTypeInfo> _messageTypes = new Dictionary<Type, IRosMessageTypeInfo>();
        private readonly IDictionary<string, IRosMessageTypeInfo> _rosTypes = new Dictionary<string, IRosMessageTypeInfo>();

        public IRosMessageTypeInfo this[Type mappedType]
        {
            get => _messageTypes[mappedType];
        }

        public IRosMessageTypeInfo this[string rosTypeName]
        {
            get => _rosTypes[rosTypeName];
        }

        public IRosMessageTypeInfo GetOrCreateMessageTypeInfo(Type type)
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
            
            var messageInfo = CreateMessageTypeInfo(type, messageDescriptor);
            
            _messageTypes.Add(messageInfo.Type, messageInfo);
            _rosTypes.Add(messageInfo.RosType.ToString(), messageInfo);
            
            return messageInfo;
        }

        private RosMessageTypeInfo CreateMessageTypeInfo(Type mappedType, RosMessageDescriptor messageDescriptor)
        {
            var dependencies = new List<IRosMessageTypeInfo>();
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

            return new RosMessageTypeInfo(mappedType, messageDescriptor, dependencies);
        }
    }
}