using System;
using System.Collections.Generic;

namespace Joanneum.Robotics.Ros.MessageBase
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RosMessageTypeAttribute : Attribute
    {
        public string RosType { get; set; }

        public RosMessageTypeAttribute()
        {
        }

        public RosMessageTypeAttribute(string rosType)
        {
            RosType = rosType;
        }
    }
}