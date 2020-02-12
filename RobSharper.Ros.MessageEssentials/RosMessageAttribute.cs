using System;

namespace RobSharper.Ros.MessageEssentials
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RosMessageAttribute : Attribute
    {
        public string RosType { get; set; }

        public RosMessageAttribute()
        {
        }

        public RosMessageAttribute(string rosType)
        {
            RosType = rosType;
        }
    }
}