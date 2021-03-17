namespace RobSharper.Ros.MessageEssentials
{
    public class RosActionMessageAttribute : RosMessageBaseAttribute
    {
        public override string MessageType { get; }
        
        public string ActionName { get; }

        public ActionMessageKind MessageKind { get; }

        public RosActionMessageAttribute(string rosActionName, ActionMessageKind kind)
        {
            ActionName = rosActionName;
            MessageKind = kind;
            MessageType = rosActionName + kind;
        }
    }
}