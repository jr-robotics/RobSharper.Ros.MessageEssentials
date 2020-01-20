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

            descriptorBuilder.SetRosType(messageTypeAttribute.RosPackage, messageTypeAttribute.RosType);
            
            descriptorBuilder.SetMappedType(type);
            
            // Get Fields
            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var rosFieldAttribute = property.GetCustomAttributes(false)
                    .OfType<RosMessageFieldAttribute>()
                    .FirstOrDefault();
                
                if (rosFieldAttribute == null)
                    continue;

                var rosType = RosType.Create(rosFieldAttribute.RosType, property.PropertyType);
                
                var fieldDescriptor = new RosMessageFieldDescriptor(rosFieldAttribute.Index, rosType, rosFieldAttribute.RosIdentifier);
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