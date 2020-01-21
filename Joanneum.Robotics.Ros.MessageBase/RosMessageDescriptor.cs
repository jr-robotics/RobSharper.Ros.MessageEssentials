using System;
using System.Collections.Generic;
using System.Linq;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosMessageDescriptor
    {
        private string _messageDefinition;
        public RosType RosType { get; }
        
        public IEnumerable<RosMessageFieldDescriptor> Fields { get; }

        public IEnumerable<RosMessageConstantDescriptor> Constants { get; }

        public string MessageDefinition
        {
            get
            {
                if (_messageDefinition != null)
                    return _messageDefinition;

                _messageDefinition = string.Join("\n",
                    Constants.Select(x => x.ToString())
                        .Union(Fields.Select(x => x.ToString())));
                    
                return _messageDefinition;
            }
        }

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

        public override string ToString()
        {
            return MessageDefinition;
        }
    }
}