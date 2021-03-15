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

            var rosType = RosType.Parse(messageTypeAttribute.MessageName);
            descriptorBuilder.SetRosType(rosType);
            
            // Get Fields from properties
            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var rosFieldAttribute = GetRosMessageFieldAttribute(property);
                
                if (rosFieldAttribute == null)
                    continue;

                var rosFieldType = GetRosFieldType(rosFieldAttribute.RosType, rosType.PackageName);
                var rosIdentifier = rosFieldAttribute.RosIdentifier ?? property.Name;

                var fieldDescriptor = new PropertyRosMessageFieldDescriptor(rosFieldAttribute.Index, rosFieldType,
                    rosIdentifier, property);
                
                descriptorBuilder.Add(fieldDescriptor);
            }
            
            // Get Fields from fields
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                var rosFieldAttribute = GetRosMessageFieldAttribute(field);
                
                if (rosFieldAttribute == null)
                    continue;

                var rosFieldType = GetRosFieldType(rosFieldAttribute.RosType, rosType.PackageName);
                var rosIdentifier = rosFieldAttribute.RosIdentifier ?? field.Name;

                var fieldDescriptor = new FieldRosMessageFieldDescriptor(rosFieldAttribute.Index, rosFieldType,
                    rosIdentifier, field);
                
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

        private static RosType GetRosFieldType(string rosType, string fallbackPackage)
        {
            var rosFieldType = RosType.Parse(rosType);

            // Add package definition for intra package type refs
            if (!rosFieldType.IsFullQualified)
            {
                rosFieldType = rosFieldType.ToFullQualifiedType(fallbackPackage);
            }

            return rosFieldType;
        }

        private static RosMessageFieldAttribute GetRosMessageFieldAttribute(MemberInfo member)
        {
            var rosFieldAttribute = member.GetCustomAttributes(false)
                .OfType<RosMessageFieldAttribute>()
                .FirstOrDefault();
            return rosFieldAttribute;
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