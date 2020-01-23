using System;
using System.Reflection;
using System.Text;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosMessageFieldDescriptor
    {
        public int Index { get;}
        public RosType RosType { get; }
        public string RosIdentifier { get; }
        public PropertyInfo MappedProperty { get; }
        
        public RosMessageFieldDescriptor(int index, RosType rosType, string rosIdentifier, PropertyInfo mappedProperty)
        {
            Index = index;
            RosType = rosType ?? throw new ArgumentNullException(nameof(rosType));
            RosIdentifier = rosIdentifier ?? throw new ArgumentNullException(nameof(rosIdentifier));
            MappedProperty = mappedProperty ?? throw new ArgumentNullException(nameof(mappedProperty));
        }

        public override string ToString()
        {
            return $"{RosType} {RosIdentifier}";
        }
    }
}