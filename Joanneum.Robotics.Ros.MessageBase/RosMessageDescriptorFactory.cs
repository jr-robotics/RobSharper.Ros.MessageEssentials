using System;
using System.Linq;
using System.Reflection;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public static class RosMessageDescriptorFactory
    {
        public static RosMessageDescriptor Create<TMessageType>() where TMessageType : class
        {
            return Create(typeof(TMessageType));
        }
        
        public static RosMessageDescriptor Create(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            
            var messageTypeAttribute = type.GetCustomAttributes(false)
                .OfType<RosMessageTypeAttribute>()
                .FirstOrDefault();
            
            if (messageTypeAttribute == null)
                throw new NotSupportedException();
            
            var descriptorBuilder = new RosMessageDescriptorBuilder();

            var rosType = RosType.Parse(messageTypeAttribute.RosType);
            descriptorBuilder.SetRosType(rosType);
            
            descriptorBuilder.SetMappedType(type);
            
            // Get Fields
            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var rosFieldAttribute = property.GetCustomAttributes(false)
                    .OfType<RosMessageFieldAttribute>()
                    .FirstOrDefault();
                
                if (rosFieldAttribute == null)
                    continue;

                var rosFieldType = RosType.Parse(rosFieldAttribute.RosType);
                var fieldDescriptor = new RosMessageFieldDescriptor(rosFieldAttribute.Index, rosFieldType, rosFieldAttribute.RosIdentifier, property.PropertyType);
                descriptorBuilder.AddField(fieldDescriptor);
            }

            // TODO Get Constants
            
            
            return descriptorBuilder.Build();
        }

        public static bool CanCreate<TMessageType>() where TMessageType : class
        {
            return CanCreate(typeof(TMessageType));
        }
        
        public static bool CanCreate(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type
                .GetCustomAttributes(typeof(RosMessageTypeAttribute), false)
                .Any();
        }
    }
}