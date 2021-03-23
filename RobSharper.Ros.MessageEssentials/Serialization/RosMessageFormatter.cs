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
                throw new NotSupportedException("MessageTypeInfo is no DescriptorBasedMessageTypeInfo");

            var fields = messageInfo.MessageDescriptor.Fields;

            foreach (var field in fields)
            {
                try
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
                catch (Exception e)
                {
                    if (e is RosFieldSerializationException rosException)
                    {
                        rosException.AddLeadingRosIdentifier(field.RosIdentifier);
                        throw;
                    }
                    else
                    {
                        throw new RosFieldSerializationException(RosFieldSerializationException.SerializationOperation.Serialize, field.RosIdentifier, e);
                    }
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

        private void SerializeArray(SerializationContext context, RosBinaryWriter writer, RosType rosType, Type memberType,
            object value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            
            var collection = value as ICollection;

            if (collection == null)
                throw new InvalidCastException("Value does not implement System.ICollection.");

            var elementCount = collection.Count;

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

            var elementType = GetGenericElementType(memberType);

            var index = -1;
            foreach (var item in collection)
            {
                index++;

                try
                {
                    SerializeValue(context, writer, rosType, elementType, item);
                }
                catch (Exception e)
                {
                    var indexString = $"[{index}]";
                    if (e is RosFieldSerializationException rosException)
                    {
                        rosException.AddLeadingRosIdentifier(indexString);
                        throw;
                    }
                    else
                    {
                        throw new RosFieldSerializationException(RosFieldSerializationException.SerializationOperation.Serialize, indexString, e);
                    }
                }
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
                try
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
                catch (Exception e)
                {
                    if (e is RosFieldSerializationException rosException)
                    {
                        rosException.AddLeadingRosIdentifier(field.RosIdentifier);
                        throw;
                    }
                    else
                    {
                        throw new RosFieldSerializationException(RosFieldSerializationException.SerializationOperation.Deserialize, field.RosIdentifier, e);
                    }
                }
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
            Type memberType)
        {
            int length;

            if (rosType.IsFixedSizeArray)
                length = rosType.ArraySize;
            else
                length = reader.ReadInt32();

            var elementType = GetGenericElementType(memberType);
            var listType = typeof(List<>).MakeGenericType(elementType);

            var list = (IList) Activator.CreateInstance(listType);

            for (var i = 0; i < length; i++)
            {
                try
                {
                    var item = DeserializeValue(context, reader, rosType, elementType);
                    list.Add(item);
                }
                catch (Exception e)
                {
                    var indexString = $"[{i}]";
                    if (e is RosFieldSerializationException rosException)
                    {
                        rosException.AddLeadingRosIdentifier(indexString);
                        throw;
                    }
                    else
                    {
                        throw new RosFieldSerializationException(RosFieldSerializationException.SerializationOperation.Deserialize, indexString, e);
                    }
                }
            }

            return ConvertListToMemberType(list, memberType);
        }

        private static object ConvertListToMemberType(IList list, Type memberType)
        {
            var listType = list.GetType();
            
            // If IList<T> can be assigned to memberType, return it.
            if (memberType.IsAssignableFrom(listType))
                return list;

            // If memberType is an array, call IList<T>.ToArray().
            if (memberType.IsArray)
            {
                var toArrayMethod = list
                    .GetType()
                    .GetMethod("ToArray");

                if (toArrayMethod == null)
                    throw new InvalidOperationException($"Could not reflect ToArray() method from {list.GetType()}");
                
                return toArrayMethod.Invoke(list, null);
            }

            // If memberType has a constructor with 1 argument accepting the list, use it!
            var memberTypeConstructor = memberType.GetConstructor(new[] {listType});
            
            if (memberTypeConstructor != null)
            {
                return memberTypeConstructor.Invoke(new object[] {list});
            }
            
            throw new InvalidOperationException($"Cannot assign {list.GetType()} to type {memberType}");
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