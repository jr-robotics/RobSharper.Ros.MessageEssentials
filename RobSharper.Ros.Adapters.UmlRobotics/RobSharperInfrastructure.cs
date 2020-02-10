using RobSharper.Ros.MessageBase;
using RobSharper.Ros.MessageBase.Serialization;

namespace RobSharper.Ros.Adapters.UmlRobotics
{
    public class RobSharperInfrastructure
    {
        private static MessageTypeRegistry _messageTypeRegistry;
        private static RosMessageSerializer _serializer;

        public static MessageTypeRegistry MessageTypeRegistry
        {
            get
            {
                if (_messageTypeRegistry == null)
                {
                    lock (typeof(RobSharperInfrastructure))
                    {
                        if (_messageTypeRegistry == null)
                        {
                            _messageTypeRegistry = new MessageTypeRegistry();
                            
                            // Configure support for UML.Robotics.RosMessage classes
                            _messageTypeRegistry.RosMessageTypeInfoFactories.Add(new UmlRoboticsMessageTypeInfoFactory());
                        }
                    }
                }

                return _messageTypeRegistry;
            }
            
            set => _messageTypeRegistry = value;
        }

        public static RosMessageSerializer Serializer
        {
            get
            {
                if (_serializer == null)
                {
                    lock (typeof(RobSharperInfrastructure))
                    {
                        if (_serializer == null)
                        {
                            _serializer = new RosMessageSerializer(MessageTypeRegistry);
                            _serializer.MessageFormatters.Add(new UmlRoboticsRosMessageFormatter());
                        }
                    }
                }

                return _serializer;
            }
            
            set => _serializer = value;
        }
    }
}