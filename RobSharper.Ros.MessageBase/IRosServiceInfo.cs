namespace RobSharper.Ros.MessageBase
{
    public interface IRosServiceInfo
    {
        RosType Type { get; }
        
        IMessageTypeInfo Request { get; }
        IMessageTypeInfo Response { get; }
        
        string MD5Sum { get; }
    }
}