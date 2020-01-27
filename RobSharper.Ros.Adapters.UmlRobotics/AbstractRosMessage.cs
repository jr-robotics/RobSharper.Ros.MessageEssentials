using RobSharper.Ros.MessageBase;
using Uml.Robotics.Ros;
using MessageTypeRegistry = RobSharper.Ros.MessageBase.MessageTypeRegistry;

namespace RobSharper.Ros.Adapters.UmlRobotics
{
    public abstract class AbstractRosMessage : RosMessage
    {
        private IMessageTypeInfo _typeInfo;

        protected IMessageTypeInfo TypeInfo
        {
            get
            {
                if (_typeInfo == null)
                {
                    _typeInfo = MessageTypeRegistry.Instance
                        .GetOrCreateMessageTypeInfo(GetType());
                }

                return _typeInfo;
            }
        }

        public override string MessageType => TypeInfo.MessageDescriptor.RosType.ToString();
        
        protected AbstractRosMessage() : base()
        {
        }
        protected AbstractRosMessage(byte[] serializedMessage) : base(serializedMessage)
        {
        }

        protected AbstractRosMessage(byte[] serializedMessage, ref int currentIndex) : base(serializedMessage)
        {
        }
        
        public override string MD5Sum()
        {
            return TypeInfo.MD5Sum;
        }

        public override bool HasHeader()
        {
            return TypeInfo.MessageDescriptor.HasHader;
        }

        // Omitted IsMetaType() because it seems that it is not used anywhere.
        // Whatever IsMetaType() means. It seems that it is indicating, that the message has properties
        // of an external type (i.e. no primitive ROS type, Message of same package or Type defined in 
        // MessageBase package.

        // Omitted IsServiceComponent() because this is only true for .srv messages

        public override string MessageDefinition()
        {
            return TypeInfo.MessageDefinition;
        }
    }
}