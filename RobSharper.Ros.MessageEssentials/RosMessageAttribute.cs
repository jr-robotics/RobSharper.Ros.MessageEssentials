using System;

namespace RobSharper.Ros.MessageEssentials
{
    public sealed class RosMessageAttribute : RosMessageBaseAttribute
    {
        private string _messageName;
        
        [Obsolete("This property was renamed to MessageName")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public string RosType
        {
            get => _messageName;
            set => _messageName = value;
        }

        public override string MessageName => _messageName;

        [Obsolete("Use constructor with message type")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public RosMessageAttribute() : base()
        {
        }

        public RosMessageAttribute(string rosMessageName)
        {
            _messageName = rosMessageName;
        }
    }
}