using System;
using RobSharper.Ros.MessageBase;
using RobSharper.Ros.MessageBase.Serialization;
using Uml.Robotics.Ros;

namespace RobSharper.Ros.Adapters.UmlRobotics
{
    /// <summary>
    /// Formatter used for serializing and deserializing of UML Robotics ROS messages.
    /// </summary>
    public class UmlRoboticsRosMessageFormatter : IRosMessageFormatter
    {
        public bool CanSerialize(IMessageTypeInfo typeInfo)
        {
            return  typeof(RosMessage).IsAssignableFrom(typeInfo.MessageDescriptor.Type);
        }

        public void Serialize(SerializationContext context, RosBinaryWriter writer, IMessageTypeInfo messageTypeInfo, object o)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            if (o == null) throw new ArgumentNullException(nameof(o));

            var rosMessage = o as RosMessage;
            
            if(rosMessage == null) throw new NotSupportedException();

            var serializedMessage = rosMessage.Serialize();
            writer.Write(serializedMessage);
        }

        public object Deserialize(SerializationContext context, RosBinaryReader reader, IMessageTypeInfo messageTypeInfo)
        {
            if (messageTypeInfo == null) throw new ArgumentNullException(nameof(messageTypeInfo));
            
            var result = (RosMessage) Activator.CreateInstance(messageTypeInfo.MessageDescriptor.Type);

            var currentPosition = reader.BaseStream.Position;
            var bytesRead = 0;

            try
            {
                var buffer = reader.ReadBytes((int) (reader.BaseStream.Length - currentPosition));
                result.Deserialize(buffer, ref bytesRead);
            }
            finally
            {
                reader.BaseStream.Position = currentPosition + bytesRead;
            }
            
            return result;
        }
    }
}