using System;
using System.Collections.Generic;
using System.Linq;

namespace RobSharper.Ros.MessageEssentials
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

        public bool HasHader => Fields.Any(f => f.RosType.IsHeaderType);

        public RosMessageDescriptor(RosType rosType, IEnumerable<RosMessageFieldDescriptor> fields,
            IEnumerable<RosMessageConstantDescriptor> constants)
        {
            RosType = rosType ?? throw new ArgumentNullException(nameof(rosType));
            
            if (fields == null)
                fields = Enumerable.Empty<RosMessageFieldDescriptor>();
            
            if (constants == null)
                constants = Enumerable.Empty<RosMessageConstantDescriptor>();

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