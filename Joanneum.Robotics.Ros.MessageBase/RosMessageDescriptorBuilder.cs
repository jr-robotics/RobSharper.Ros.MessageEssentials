using System;
using System.Collections.Generic;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosMessageDescriptorBuilder
    {
        private IList<RosMessageFieldDescriptor> _fields = new List<RosMessageFieldDescriptor>();
        private RosType _rosType;
        private Type _mappedType;

        public void AddField(RosMessageFieldDescriptor fieldDescriptor)
        {
            if (fieldDescriptor == null) throw new ArgumentNullException(nameof(fieldDescriptor));
            _fields.Add(fieldDescriptor);
        }

        public RosMessageDescriptor Build()
        {
            return new RosMessageDescriptor(_rosType, _fields);
        }

        public void SetRosType(RosType rosType)
        {   
            _rosType = rosType ?? throw new ArgumentNullException(nameof(rosType));
        }

        public void SetMappedType(Type type)
        {
            _mappedType = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
}