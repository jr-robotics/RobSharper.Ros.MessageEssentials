using System;

namespace RobSharper.Ros.MessageEssentials
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RosMessageAttribute : Attribute
    {
        private string _messageName;
        
        [Obsolete("This property was renamed to MessageName")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public string RosType
        {
            get => _messageName;
            set => _messageName = value;
        }

        public string MessageName
        {
            get => _messageName;
            protected set => _messageName = value;
        } 

        [Obsolete("Use constructor with message type")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public RosMessageAttribute()
        {
        }

        public RosMessageAttribute(string rosMessageName)
        {
            _messageName = rosMessageName;
        }
    }
}