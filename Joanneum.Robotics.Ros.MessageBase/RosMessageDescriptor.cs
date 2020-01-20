using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosMessageDescriptor
    {
        public IEnumerable<RosMessageFieldDescriptor> Fields { get; }

        public RosMessageDescriptor(IEnumerable<RosMessageFieldDescriptor> fields)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));
            
            Fields = fields
                .OrderBy(f => f.Index)
                .ToList();
        }


    }
}