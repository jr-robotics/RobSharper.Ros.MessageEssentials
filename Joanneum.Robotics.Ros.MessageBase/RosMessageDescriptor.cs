using System;
using System.Collections.Generic;
using System.Linq;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosMessageDescriptor
    {
        public RosType RosType { get; }
        
        public IEnumerable<RosMessageFieldDescriptor> Fields { get; }

        public IEnumerable<RosMessageConstantDescriptor> Constants { get; }

        public RosMessageDescriptor(RosType rosType, IEnumerable<RosMessageFieldDescriptor> fields,
            IEnumerable<RosMessageConstantDescriptor> constants)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));
            if (constants == null) throw new ArgumentNullException(nameof(constants));
            RosType = rosType ?? throw new ArgumentNullException(nameof(rosType));

            Fields = fields
                .OrderBy(f => f.Index)
                .ToList();

            Constants = constants
                .OrderBy(c => c.Index)
                .ToList();
        }
    }
}