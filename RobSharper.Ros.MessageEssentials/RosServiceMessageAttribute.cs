namespace RobSharper.Ros.MessageEssentials
{
    public sealed class RosServiceMessageAttribute : RosMessageAttribute
    {
        public string ServiceName { get; }

        public ServiceMessageKind MessageKind { get; }

        public RosServiceMessageAttribute(string rosServiceName, ServiceMessageKind kind) : 
            base(rosServiceName + kind.ToString())
        {
            ServiceName = rosServiceName;
            MessageKind = kind;
        }
    }
}