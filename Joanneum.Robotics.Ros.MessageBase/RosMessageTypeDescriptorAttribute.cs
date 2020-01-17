using System;
using System.Collections.Generic;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosMessageTypeDescriptorAttribute : Attribute
    {
        public string RosPackage { get; set; }
        public string RosType { get; set; }

        public RosMessageTypeDescriptorAttribute()
        {
        }

        public RosMessageTypeDescriptorAttribute(string rosPackage, string rosType)
        {
            RosPackage = rosPackage;
            RosType = rosType;
        }
    }
}