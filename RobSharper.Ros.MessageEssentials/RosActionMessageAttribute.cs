namespace RobSharper.Ros.MessageEssentials
{
    public sealed class RosActionMessageAttribute : RosMessageBaseAttribute
    {
        public override string MessageName { get; }
        
        public string ServiceName { get; }

        public ActionMessageKind MessageKind { get; }

        public RosActionMessageAttribute(string rosServiceName, ActionMessageKind kind)
        {
            ServiceName = rosServiceName;
            MessageKind = kind;
            MessageName = rosServiceName + kind;
        }
    }
}