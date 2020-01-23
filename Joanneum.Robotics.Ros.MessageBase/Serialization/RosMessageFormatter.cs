using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Joanneum.Robotics.Ros.MessageBase.Serialization
{
    public class RosMessageFormatter : IRosMessageFormatter
    {
        public bool CanSerialize(IMessageTypeInfo typeInfo)
        {
            return typeInfo is MessageTypeInfo;
        }
        
        public void Serialize(SerializationContext context, IMessageTypeInfo messageTypeInfo, object o)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (o == null) throw new ArgumentNullException(nameof(o));

            if (!(messageTypeInfo is MessageTypeInfo messageInfo))
                throw new NotSupportedException();

            var fields = messageInfo.MessageDescriptor.Fields;
            
            foreach (var field in fields)
            {
                var value = field.MappedProperty.GetValue(o);
                
                if (field.RosType.IsArray)
                {
                    SerializeArray(context, field.RosType, field.MappedProperty.PropertyType, value);
                }
                else
                {
                    SerializeValue(context, field.RosType, field.MappedProperty.PropertyType, value);
                }
            }
        }

        private void SerializeValue(SerializationContext context, RosType rosType, Type type, object value)
        {
            if (rosType.IsBuiltIn)
            {
                RosBuiltInTypeConverter.WriteBytes(context.Stream, type, value);
            }
            else
            {
                var typeInfo = context.MessageTypeRegistry.GetOrCreateMessageTypeInfo(type);
                IRosMessageFormatter formatter = this;
                
                // If this serializer cannot serialize the object search for serializer who can do it
                if (!CanSerialize(typeInfo))
                {
                    formatter = context.MessageFormatters.FindFormatterFor(typeInfo);

                    if (formatter == null)
                        throw new NotSupportedException($"No formatter for message {typeInfo} found.");
                }

                formatter.Serialize(context, typeInfo, value);
            }
        }

        private void SerializeArray(SerializationContext context, RosType rosType, Type type, object value)
        {
            var collection = value as ICollection;
            
            var elementCount = collection?.Count ?? 0;

            if (rosType.IsFixedSizeArray && rosType.ArraySize != elementCount)
            {
                throw new InvalidOperationException(
                    $"Expected array size of {rosType.ArraySize} but found array size of {elementCount}.");
            }

            RosBuiltInTypeConverter.WriteBytes(context.Stream, elementCount);
            
            if (elementCount == 0)
                return;
            
            type = type
                .GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(t => t.GetGenericArguments()[0])
                .FirstOrDefault();
            
            foreach (var item in collection)
            {
                SerializeValue(context, rosType, type, item);
            }
        }

        public object Deserialize(SerializationContext context, IMessageTypeInfo messageTypeInfo)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (messageTypeInfo == null) throw new ArgumentNullException(nameof(messageTypeInfo));
            
            if (!(messageTypeInfo is MessageTypeInfo messageInfo))
                throw new NotSupportedException();

            
            var result = Activator.CreateInstance(messageInfo.MessageDescriptor.Type);
            
            var fields = messageInfo.MessageDescriptor.Fields;
            
            foreach (var field in fields)
            {
                object fieldValue;
                if (field.RosType.IsArray)
                {
                    fieldValue = DeserializeArray(context, field.RosType, field.MappedProperty.PropertyType);
                }
                else
                {
                    fieldValue = DeserializeValue(context, field.RosType, field.MappedProperty.PropertyType);
                }

                field.MappedProperty.SetValue(result, fieldValue);
            }

            return result;
        }

        private object DeserializeValue(SerializationContext context, RosType rosType, Type type)
        {
            if (rosType.IsBuiltIn)
            {
                return RosBuiltInTypeConverter.ReadValue(context.Stream, type);
            }
            else
            {
                var typeInfo = context.MessageTypeRegistry.GetOrCreateMessageTypeInfo(type);
                IRosMessageFormatter formatter = this;
                
                // If this serializer cannot serialize the object search for serializer who can do it
                if (!CanSerialize(typeInfo))
                {
                    formatter = context.MessageFormatters.FindFormatterFor(typeInfo);

                    if (formatter == null)
                        throw new NotSupportedException($"No formatter for message {typeInfo} found.");
                }

                return formatter.Deserialize(context, typeInfo);
            }
        }

        private object DeserializeArray(SerializationContext context, RosType rosType, Type arrayType)
        {
            var length = RosBuiltInTypeConverter.ReadInt32(context.Stream);

            if (rosType.IsFixedSizeArray && rosType.ArraySize != length)
            {
                throw new InvalidOperationException(
                    $"Expected array size of {rosType.ArraySize} but found array size of {length}.");
            }
            
            var elementType = arrayType
                .GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(t => t.GetGenericArguments()[0])
                .FirstOrDefault();

            var listType = typeof(List<>)
                .MakeGenericType(elementType);
            
            if (!arrayType.IsAssignableFrom(listType))
                throw new InvalidOperationException($"Cannot assign {listType} to type {arrayType}");
            
            var result = (IList) Activator.CreateInstance(listType);

            for (var i = 0; i < length; i++)
            {
                var item = DeserializeValue(context, rosType, elementType);
                result.Add(item);
            }

            return result;
        }
    }
}