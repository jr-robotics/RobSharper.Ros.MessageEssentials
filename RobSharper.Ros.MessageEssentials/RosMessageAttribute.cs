using System;

namespace RobSharper.Ros.MessageEssentials
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RosMessageAttribute : Attribute
    {
        private string _messageType;
        
        [Obsolete("This property was renamed to MessageType")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public string RosType
        {
            get => _messageType;
            set => _messageType = value;
        }

        public string MessageType
        {
            get => _messageType;
            protected set => _messageType = value;
        } 

        [Obsolete("Use constructor with message type")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public RosMessageAttribute()
        {
        }

        public RosMessageAttribute(string rosMessageType)
        {
            _messageType = rosMessageType;
        }
    }
}