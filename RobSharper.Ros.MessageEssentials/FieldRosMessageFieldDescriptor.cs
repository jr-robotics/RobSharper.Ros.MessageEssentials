using System;
using System.Reflection;

namespace RobSharper.Ros.MessageEssentials
{
    public class FieldRosMessageFieldDescriptor : RosMessageFieldDescriptor
    {
        public FieldInfo MappedField { get; }

        public override Type Type => MappedField.FieldType;

        public FieldRosMessageFieldDescriptor(int index, RosType rosType, string rosIdentifier,
            FieldInfo mappedField) : base(index, rosType, rosIdentifier)
        {
            MappedField = mappedField ?? throw new ArgumentNullException(nameof(mappedField));
        }
        
        public override object GetValue(object o)
        {
            return MappedField.GetValue(o);
        }

        public override void SetValue(object obj, object value)
        {
            MappedField.SetValue(obj, value);
        }
    }
}