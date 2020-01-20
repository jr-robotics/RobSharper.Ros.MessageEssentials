using System;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosMessageFieldAttribute : Attribute
    {
        public int Index { get; set; }
        public string RosType { get; set; }
        public string RosIdentifier { get; set; }

        public RosMessageFieldAttribute() {}
        
        public RosMessageFieldAttribute(int index, string rosType, string rosIdentifier)
        {
            Index = index;
            RosType = rosType;
            RosIdentifier = rosIdentifier;
        }
    }

    public class RosMessageFieldDescriptor
    {
        public int Index { get;}
        public RosType RosType { get; }
        public string RosIdentifier { get; }
        
        public Type MappedType { get; }
        
        public RosMessageFieldDescriptor(int index, RosType rosType, string rosIdentifier, Type mappedType)
        {
            if (index <= 0) throw new ArgumentOutOfRangeException(nameof(index));
            
            Index = index;
            RosType = rosType ?? throw new ArgumentNullException(nameof(rosType));
            RosIdentifier = rosIdentifier ?? throw new ArgumentNullException(nameof(rosIdentifier));
            MappedType = mappedType ?? throw new ArgumentNullException(nameof(mappedType));
        }
    }
}