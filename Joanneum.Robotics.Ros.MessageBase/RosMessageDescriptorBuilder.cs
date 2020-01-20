using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosMessageDescriptorBuilder
    {
        private IList<RosMessageFieldDescriptor> _fields = new List<RosMessageFieldDescriptor>();

        public void AddField(RosMessageFieldDescriptor fieldDescriptor)
        {
            if (fieldDescriptor == null) throw new ArgumentNullException(nameof(fieldDescriptor));
            _fields.Add(fieldDescriptor);
        }

        public RosMessageDescriptor Build()
        {
            return new RosMessageDescriptor(_fields);
        }
    }
}