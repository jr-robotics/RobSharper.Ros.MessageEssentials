using Uml.Robotics.Ros;

namespace RobSharper.Ros.Adapters.UmlRobotics
{
    public class MessageTypeInfoProvider<TMessage> : MessageTypeInfoProvider
        where TMessage : RosMessage
    {
        public MessageTypeInfoProvider() : base(typeof(TMessage))
        {
        }
    }
}