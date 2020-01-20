using System;
using System.Collections.Generic;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosMessageTypeAttribute : Attribute
    {
        public string RosPackage { get; set; }
        public string RosType { get; set; }

        public RosMessageTypeAttribute()
        {
        }

        public RosMessageTypeAttribute(string rosPackage, string rosType)
        {
            RosPackage = rosPackage;
            RosType = rosType;
        }
    }
}