using System;

namespace RobSharper.Ros.MessageBase
{
    public interface IRosMessageMetadata
    {
        RosType Type { get; }
        
        string MD5Sum { get; }
        
        string MessageDefinition { get; }        
    }
    
    public interface IRosServiceMetadata
    {
        IRosMessageMetadata Request { get; }
        IRosMessageMetadata Response { get; }
        
        RosType Type { get; }
        
        string MD5Sum { get; }
        
        string MessageDefinition { get; }
    }

    public class RosServiceMetadata : IRosServiceMetadata
    {
        private string _md5Sum;
        private string _messageDefinition;
        
        public IRosMessageMetadata Request { get; }
        public IRosMessageMetadata Response { get; }
        public RosType Type { get; }
        
        public string MD5Sum
        {
            get
            {
                if (_md5Sum == null)
                {
                    _md5Sum = CalculateMd5Sum();
                }
                
                return _md5Sum;
            }
        }

        public string MessageDefinition
        {
            get
            {
                if (_messageDefinition == null)
                {
                    _messageDefinition = CreateMessageDefinition();
                }
                
                return _messageDefinition;
            }
        }

        public RosServiceMetadata(RosType type, IRosMessageMetadata request, IRosMessageMetadata response)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Request = request ?? throw new ArgumentNullException(nameof(request));
            Response = response ?? throw new ArgumentNullException(nameof(response));
        }

        private string CalculateMd5Sum()
        {
            throw new NotImplementedException();
        }

        private string CreateMessageDefinition()
        {
            throw new NotImplementedException();
        }
    }
}