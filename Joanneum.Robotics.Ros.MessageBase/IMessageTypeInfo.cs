namespace Joanneum.Robotics.Ros.MessageBase
{
    public interface IMessageTypeInfo
    {
        RosType Type { get; }
        
        string MD5Sum { get; }
        
        string MessageDefinition { get; }
        
        bool HasHeader { get; }
    }
}