using System.Text;

namespace RobSharper.Ros.MessageBase
{
    public interface IRosServiceInfo
    {
        RosType RosType { get; }
        
        IRosMessageTypeInfo Request { get; }
        IRosMessageTypeInfo Response { get; }
        
        string MD5Sum { get; }
    }
}