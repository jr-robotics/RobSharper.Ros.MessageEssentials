using System;

namespace Joanneum.Robotics.Ros.MessageBase
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
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
}