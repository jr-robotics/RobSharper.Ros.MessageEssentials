using System;
using System.Collections.Generic;

namespace RobSharper.Ros.MessageBase
{
    public class RosMessageDescriptorBuilder
    {
        private RosType _rosType;
        private readonly IList<RosMessageFieldDescriptor> _fields = new List<RosMessageFieldDescriptor>();
        private readonly IList<RosMessageConstantDescriptor> _constants = new List<RosMessageConstantDescriptor>();

        public void SetRosType(RosType rosType)
        {   
            _rosType = rosType ?? throw new ArgumentNullException(nameof(rosType));
        }

        public void Add(RosMessageConstantDescriptor constantDescriptor)
        {
            if (constantDescriptor == null) throw new ArgumentNullException(nameof(constantDescriptor));
            _constants.Add(constantDescriptor);
        }
        
        public void Add(RosMessageFieldDescriptor fieldDescriptor)
        {
            if (fieldDescriptor == null) throw new ArgumentNullException(nameof(fieldDescriptor));
            _fields.Add(fieldDescriptor);
        }

        public RosMessageDescriptor Build()
        {
            return new RosMessageDescriptor(_rosType, _fields, _constants);
        }
    }
}