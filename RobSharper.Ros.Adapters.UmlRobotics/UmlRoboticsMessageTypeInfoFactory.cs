using System;
using System.Linq;
using RobSharper.Ros.MessageEssentials;
using Uml.Robotics.Ros;

namespace RobSharper.Ros.Adapters.UmlRobotics
{
    public class UmlRoboticsMessageTypeInfoFactory : IRosMessageTypeInfoFactory
    {
        public bool CanCreate(Type messageType)
        {
            var isUmlRoboticsRosMessage =  typeof(RosMessage).IsAssignableFrom(messageType);
            var hasRobSHarperAttributes = messageType
                .GetCustomAttributes(typeof(RosMessageAttribute), false)
                .Any();

            return isUmlRoboticsRosMessage && !hasRobSHarperAttributes;
        }

        public IRosMessageTypeInfo Create(Type messageType)
        {
            var protoObject = Activator.CreateInstance(messageType) as RosMessage;
            
            if (protoObject == null)
                throw new ArgumentException($"RosMessageType must have {typeof(RosMessage)} as base type.");

            return new UmlRoboticsMessageTypeInfo(protoObject);
        }
    }
}