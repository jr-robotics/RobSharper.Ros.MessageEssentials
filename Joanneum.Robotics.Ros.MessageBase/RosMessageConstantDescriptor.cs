using System;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosMessageConstantDescriptor
    {
        public int Index { get; }
        public RosType RosType { get; }
        public string RosIdentifier { get; }
        public object Value { get; }

        public RosMessageConstantDescriptor(int index, RosType rosType, string rosIdentifier, object value)
        {
            Index = index;
            RosType = rosType ?? throw new ArgumentNullException(nameof(rosType));
            RosIdentifier = rosIdentifier ?? throw new ArgumentNullException(nameof(rosIdentifier));
            Value = value ?? throw new ArgumentNullException(nameof(value));

            if (!rosType.IsBuiltIn || rosType.IsArray)
            {
                throw new ArgumentOutOfRangeException(nameof(rosType), $"ROS type {rosType} is no supported constant type");
            }
        }

        public override string ToString()
        {
            return $"{RosType} {RosIdentifier}={Value}";
        }
    }
}