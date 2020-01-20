using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosMessageDescriptorBuilder
    {
        private IList<RosMessageFieldDescriptor> _fields = new List<RosMessageFieldDescriptor>();
        private string _rosPackage;
        private string _rosType;
        private Type _mappedType;

        public void AddField(RosMessageFieldDescriptor fieldDescriptor)
        {
            if (fieldDescriptor == null) throw new ArgumentNullException(nameof(fieldDescriptor));
            _fields.Add(fieldDescriptor);
        }

        public RosMessageDescriptor Build()
        {
            var rosType = RosType.Create(_rosPackage, _rosType, _mappedType);
            return new RosMessageDescriptor(rosType, _fields);
        }

        public void SetRosType(string rosPackage, string rosType)
        {   
            _rosPackage = rosPackage ?? throw new ArgumentNullException(nameof(rosPackage));
            _rosType = rosType ?? throw new ArgumentNullException(nameof(rosType));
        }

        public void SetMappedType(Type type)
        {
            _mappedType = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
}