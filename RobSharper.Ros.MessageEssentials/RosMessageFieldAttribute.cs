using System;

namespace RobSharper.Ros.MessageEssentials
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RosMessageFieldAttribute : Attribute
    {
        public int Index { get; set; }
        public string RosType { get; set; }
        public string RosIdentifier { get; set; }

        public RosMessageFieldAttribute() {}
        
        [Obsolete("Use alternative constructor with reordered arguments instead")]
        public RosMessageFieldAttribute(int index, string rosType, string rosIdentifier)
        {
            Index = index;
            RosType = rosType;
            RosIdentifier = rosIdentifier;
        }
        
        public RosMessageFieldAttribute(string rosType, string rosIdentifier, int index)
        {
            Index = index;
            RosType = rosType;
            RosIdentifier = rosIdentifier;
        }
    }
}