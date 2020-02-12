using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RobSharper.Ros.MessageEssentials
{
    public static class AttributeBasedRosMessageDescriptorFactory
    {
        public static RosMessageDescriptor Create<TMessageType>() where TMessageType : class
        {
            return Create(typeof(TMessageType));
        }
        
        public static RosMessageDescriptor Create(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            
            var messageTypeAttribute = type.GetCustomAttributes(false)
                .OfType<RosMessageAttribute>()
                .FirstOrDefault();
            
            if (messageTypeAttribute == null)
                throw new NotSupportedException();
            
            var descriptorBuilder = new RosMessageDescriptorBuilder();

            var rosType = RosType.Parse(messageTypeAttribute.RosType);
            descriptorBuilder.SetRosType(rosType);
            
            // Get Fields
            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var rosFieldAttribute = property.GetCustomAttributes(false)
                    .OfType<RosMessageFieldAttribute>()
                    .FirstOrDefault();
                
                if (rosFieldAttribute == null)
                    continue;

                var rosFieldType = RosType.Parse(rosFieldAttribute.RosType);
                
                // Add package definition for intra package type refs
                if (!rosFieldType.IsFullQualified)
                {
                    rosFieldType = rosFieldType.ToFullQualifiedType(rosType.PackageName);
                }
                
                var fieldDescriptor = new RosMessageFieldDescriptor(rosFieldAttribute.Index, rosFieldType, rosFieldAttribute.RosIdentifier, property);
                descriptorBuilder.Add(fieldDescriptor);
            }

            // Get Constants
            foreach (var constant in GetConstants(type))
            {
                var rosConstantAttribute = constant.GetCustomAttributes(false)
                    .OfType<RosMessageFieldAttribute>()
                    .FirstOrDefault();

                if (rosConstantAttribute == null)
                    continue;

                var rosConstantType = RosType.Parse(rosConstantAttribute.RosType);
                var constantValue = constant.GetValue(null);
                var constantDescriptor = new RosMessageConstantDescriptor(rosConstantAttribute.Index, rosConstantType,
                    rosConstantAttribute.RosIdentifier, constantValue);

                descriptorBuilder.Add(constantDescriptor);
            }
            
            
            return descriptorBuilder.Build();
        }

        private static IEnumerable<FieldInfo> GetConstants(Type type)
        {
            return type
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                .ToList();
        }

        public static bool CanCreate<TMessageType>() where TMessageType : class
        {
            return CanCreate(typeof(TMessageType));
        }
        
        public static bool CanCreate(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type
                .GetCustomAttributes(typeof(RosMessageAttribute), false)
                .Any();
        }
    }
}