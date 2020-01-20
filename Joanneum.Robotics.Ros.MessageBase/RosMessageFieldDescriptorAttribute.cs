using System;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosMessageFieldDescriptorAttribute : Attribute
    {
        public int Index { get; set; }
        public string RosType { get; set; }
        public string RosIdentifier { get; set; }

        public RosMessageFieldDescriptorAttribute() {}
        
        public RosMessageFieldDescriptorAttribute(int index, string rosType, string rosIdentifier)
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
        
        public RosMessageFieldDescriptor(int index, RosType rosType, string rosIdentifier)
        {
            if (index <= 0) throw new ArgumentOutOfRangeException(nameof(index));
            
            Index = index;
            RosType = rosType ?? throw new ArgumentNullException(nameof(rosType));
            RosIdentifier = rosIdentifier ?? throw new ArgumentNullException(nameof(rosIdentifier));
        }
    }
}