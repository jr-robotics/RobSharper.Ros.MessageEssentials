namespace RobSharper.Ros.MessageEssentials
{
    public class RosServiceMessageAttribute : RosMessageBaseAttribute
    {
        public override string MessageType { get; }
        
        public string ServiceName { get; }

        public ServiceMessageKind MessageKind { get; }

        public RosServiceMessageAttribute(string rosServiceName, ServiceMessageKind kind)
        {
            ServiceName = rosServiceName;
            MessageKind = kind;
            MessageType = rosServiceName + kind;
        }
    }
}