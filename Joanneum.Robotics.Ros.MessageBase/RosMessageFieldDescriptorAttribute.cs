using System;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosMessageFieldDescriptorAttribute : Attribute
    {
        public int Order { get; set; }
        public string RosType { get; set; }
        public string RosIdentifier { get; set; }

        public RosMessageFieldDescriptorAttribute()
        {
            
        }
        
        public RosMessageFieldDescriptorAttribute(int order, string rosType, string rosIdentifier)
        {
            Order = order;
            RosType = rosType;
            RosIdentifier = rosIdentifier;
        }
    }
}