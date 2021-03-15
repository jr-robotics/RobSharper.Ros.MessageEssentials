namespace RobSharper.Ros.MessageEssentials
{
    public sealed class RosServiceMessageAttribute : RosMessageBaseAttribute
    {
        public override string MessageName { get; }
        
        public string ServiceName { get; }

        public ServiceMessageKind MessageKind { get; }

        public RosServiceMessageAttribute(string rosServiceName, ServiceMessageKind kind)
        {
            ServiceName = rosServiceName;
            MessageKind = kind;
            MessageName = rosServiceName + kind;
        }
    }
}