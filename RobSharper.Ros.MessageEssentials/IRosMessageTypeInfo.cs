using System;

namespace RobSharper.Ros.MessageEssentials
{
    public interface IRosMessageTypeInfo
    {
        RosType RosType { get; }
        
        Type Type { get; }
        
        string MD5Sum { get; }
        
        string MessageDefinition { get; }
    }
}