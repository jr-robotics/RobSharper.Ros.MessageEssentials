using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RobSharper.Ros.MessageEssentials.Serialization
{
    [Obsolete("This class was renamed. Use DescriptorBasedMessageFormatter instead.")]
    public class RosMessageFormatter : DescriptorBasedMessageFormatter
    {
    }
    
    public class DescriptorBasedMessageFormatter : IRosMessageFormatter
    {
        public bool CanSerialize(IRosMessageTypeInfo typeInfo)
        {
            return typeInfo is DescriptorBasedMessageTypeInfo;
        }

        public void Serialize(SerializationContext context, RosBinaryWriter writer, IRosMessageTypeInfo messageTypeInfo,
            object o)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (o == null) throw new ArgumentNullException(nameof(o));

            if (!(messageTypeInfo is DescriptorBasedMessageTypeInfo messageInfo))
                throw new NotSupportedException();

            var fields = messageInfo.MessageDescriptor.Fields;

            foreach (var field in fields)
            {
                var value = field.GetValue(o);

                if (field.RosType.IsArray)
                {
                    SerializeArray(context, writer, field.RosType, field.Type, value);
                }
                else
                {
                    SerializeValue(context, writer, field.RosType, field.Type, value);
                }
            }
        }

        private void SerializeValue(SerializationContext context, RosBinaryWriter writer, RosType rosType, Type type,
            object value)
        {
            if (rosType.IsBuiltIn)
            {
                writer.WriteBuiltInType(rosType, value);
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

                formatter.Serialize(context, writer, typeInfo, value);
            }
        }

        private void SerializeArray(SerializationContext context, RosBinaryWriter writer, RosType rosType, Type arrayType,
            object value)
        {
            var collection = value as ICollection;

            var elementCount = collection?.Count ?? 0;

            if (rosType.IsFixedSizeArray)
            {
                if (rosType.ArraySize != elementCount)
                    throw new InvalidOperationException(
                        $"Expected array size of {rosType.ArraySize} but found array size of {elementCount}.");
            }
            else
            {
                writer.Write(elementCount);    
            }
            
            if (elementCount == 0)
                return;

            var elementType = GetGenericElementType(arrayType);

            foreach (var item in collection)
            {
                SerializeValue(context, writer, rosType, elementType, item);
            }
        }

        public object Deserialize(SerializationContext context, RosBinaryReader reader,
            IRosMessageTypeInfo messageTypeInfo)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (messageTypeInfo == null) throw new ArgumentNullException(nameof(messageTypeInfo));

            if (!(messageTypeInfo is DescriptorBasedMessageTypeInfo messageInfo))
                throw new NotSupportedException();


            var result = Activator.CreateInstance(messageInfo.Type);

            var fields = messageInfo.MessageDescriptor.Fields;

            foreach (var field in fields)
            {
                object fieldValue;
                if (field.RosType.IsArray)
                {
                    fieldValue = DeserializeArray(context, reader, field.RosType, field.Type);
                }
                else
                {
                    fieldValue = DeserializeValue(context, reader, field.RosType, field.Type);
                }

                field.SetValue(result, fieldValue);
            }

            return result;
        }

        private object DeserializeValue(SerializationContext context, RosBinaryReader reader, RosType rosType,
            Type type)
        {
            if (rosType.IsBuiltIn)
            {
                return reader.ReadBuiltInType(rosType, type);
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

                return formatter.Deserialize(context, reader, typeInfo);
            }
        }

        private object DeserializeArray(SerializationContext context, RosBinaryReader reader, RosType rosType,
            Type arrayType)
        {
            int length;

            if (rosType.IsFixedSizeArray)
                length = rosType.ArraySize;
            else
                length = reader.ReadInt32();

            var elementType = GetGenericElementType(arrayType);

            var listType = typeof(List<>)
                .MakeGenericType(elementType);

            if (!arrayType.IsAssignableFrom(listType))
                throw new InvalidOperationException($"Cannot assign {listType} to type {arrayType}");

            var result = (IList) Activator.CreateInstance(listType);

            for (var i = 0; i < length; i++)
            {
                var item = DeserializeValue(context, reader, rosType, elementType);
                result.Add(item);
            }

            return result;
        }

        private static Type GetGenericElementType(Type arrayType)
        {
            var elementType = arrayType
                .GetInterfaces()
                .Union(new [] { arrayType})
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(t => t.GetGenericArguments()[0])
                .FirstOrDefault();

            if (elementType == null)
                throw new InvalidOperationException($"Could not retrieve element type from {arrayType}");
            return elementType;
        }
    }
}