using System;
using System.Collections.Generic;

namespace RobSharper.Ros.MessageBase
{
    public interface IRosMessageTypeInfo
    {
        RosType RosType { get; }
        
        Type Type { get; }
        
        string MD5Sum { get; }
        
        string MessageDefinition { get; }
        
        IEnumerable<IRosMessageTypeInfo> Dependencies { get; }
    }
}