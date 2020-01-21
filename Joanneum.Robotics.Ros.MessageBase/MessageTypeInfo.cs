using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class MessageTypeInfo : IMessageTypeInfo
    {
        private readonly RosMessageDescriptor _messageDescriptor;

        public RosType Type => _messageDescriptor.RosType;

        public string MD5Sum { get; }

        public string MessageDefinition => _messageDescriptor.MessageDefinition;

        public bool HasHeader { get; }

        public MessageTypeInfo(RosMessageDescriptor messageDescriptor, string md5)
        {
            _messageDescriptor = messageDescriptor ?? throw new ArgumentNullException(nameof(messageDescriptor));
            MD5Sum = md5;
        }
        
        protected bool Equals(MessageTypeInfo other)
        {
            return MD5Sum == other.MD5Sum && MessageDefinition == other.MessageDefinition && HasHeader == other.HasHeader && Equals(Type, other.Type);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MessageTypeInfo) obj);
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Type != null ? Type.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (MD5Sum != null ? MD5Sum.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (MessageDefinition != null ? MessageDefinition.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ HasHeader.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}