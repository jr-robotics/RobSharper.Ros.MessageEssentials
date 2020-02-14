using System;
using System.Reflection;

namespace RobSharper.Ros.MessageEssentials
{
    public class PropertyRosMessageFieldDescriptor : RosMessageFieldDescriptor
    {
        public PropertyInfo MappedProperty { get; }

        public override Type Type => MappedProperty.PropertyType;

        public PropertyRosMessageFieldDescriptor(int index, RosType rosType, string rosIdentifier,
            PropertyInfo mappedProperty) : base(index, rosType, rosIdentifier)
        {
            MappedProperty = mappedProperty ?? throw new ArgumentNullException(nameof(mappedProperty));
        }
        
        public override object GetValue(object o)
        {
            return MappedProperty.GetValue(o);
        }

        public override void SetValue(object obj, object value)
        {
            MappedProperty.SetValue(obj, value);
        }
    }
}