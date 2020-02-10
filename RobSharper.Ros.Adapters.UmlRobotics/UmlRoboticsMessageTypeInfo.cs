using System;
using System.Collections.Generic;
using RobSharper.Ros.MessageBase;
using Uml.Robotics.Ros;

namespace RobSharper.Ros.Adapters.UmlRobotics
{
    public class UmlRoboticsMessageTypeInfo : IRosMessageTypeInfo
    {
        private readonly RosMessage _messageProto;
        public RosType RosType { get; }
        public Type Type => _messageProto.GetType();
        public string MD5Sum => _messageProto.MD5Sum();
        public string MessageDefinition => _messageProto.MessageDefinition();
        
        public IEnumerable<IRosMessageTypeInfo> Dependencies { get; }

        public UmlRoboticsMessageTypeInfo(RosMessage messageProto)
        {
            _messageProto = messageProto ?? throw new ArgumentNullException(nameof(messageProto));
            RosType = RosType.Parse(_messageProto.MessageType);
        }
    }
}