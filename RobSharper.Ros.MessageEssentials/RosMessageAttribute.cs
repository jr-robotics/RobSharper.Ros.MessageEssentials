using System;

namespace RobSharper.Ros.MessageEssentials
{
    public sealed class RosMessageAttribute : RosMessageBaseAttribute
    {
        private string _messageType;
        
        [Obsolete("This property was renamed to MessageName")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public string RosType
        {
            get => _messageType;
            set => _messageType = value;
        }

        public override string MessageType => _messageType;

        [Obsolete("Use constructor with message type")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public RosMessageAttribute() : base()
        {
        }

        public RosMessageAttribute(string rosMessageType)
        {
            _messageType = rosMessageType;
        }
    }
}