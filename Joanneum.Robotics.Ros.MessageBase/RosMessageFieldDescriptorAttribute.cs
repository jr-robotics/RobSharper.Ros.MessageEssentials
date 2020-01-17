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
}