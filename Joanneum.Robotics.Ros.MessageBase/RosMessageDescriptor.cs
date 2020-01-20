using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosMessageDescriptor
    {
        public RosType RosType { get; }
        
        public IEnumerable<RosMessageFieldDescriptor> Fields { get; }

        public RosMessageDescriptor(RosType rosType, IEnumerable<RosMessageFieldDescriptor> fields)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));
            RosType = rosType ?? throw new ArgumentNullException(nameof(rosType));

            Fields = fields
                .OrderBy(f => f.Index)
                .ToList();
        }


    }
}