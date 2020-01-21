using System;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosMessageFieldDescriptor
    {
        public int Index { get;}
        public RosType RosType { get; }
        public string RosIdentifier { get; }
        public Type MappedType { get; }
        
        public RosMessageFieldDescriptor(int index, RosType rosType, string rosIdentifier, Type mappedType)
        {
            Index = index;
            RosType = rosType ?? throw new ArgumentNullException(nameof(rosType));
            RosIdentifier = rosIdentifier ?? throw new ArgumentNullException(nameof(rosIdentifier));
            MappedType = mappedType ?? throw new ArgumentNullException(nameof(mappedType));
        }

        public override string ToString()
        {
            return $"{RosType} {RosIdentifier}";
        }
    }
}