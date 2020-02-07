namespace RobSharper.Ros.MessageBase
{
    public interface IRosServiceInfo
    {
        RosType Type { get; }
        
        IRosMessageTypeInfo Request { get; }
        IRosMessageTypeInfo Response { get; }
        
        string MD5Sum { get; }
    }
}