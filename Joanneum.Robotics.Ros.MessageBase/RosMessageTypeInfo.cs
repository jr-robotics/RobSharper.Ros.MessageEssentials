using System;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosMessageTypeInfo
    {
        public string RosFullTypeName { get; }

        public string MD5Sum { get; }
        public string MessageDefinition { get; }
        
        public bool HasHeader { get; }
        
        protected bool Equals(RosMessageTypeInfo other)
        {
            return RosFullTypeName == other.RosFullTypeName && MD5Sum == other.MD5Sum && MessageDefinition == other.MessageDefinition && HasHeader == other.HasHeader;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RosMessageTypeInfo) obj);
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (RosFullTypeName != null ? RosFullTypeName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (MD5Sum != null ? MD5Sum.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (MessageDefinition != null ? MessageDefinition.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ HasHeader.GetHashCode();
                return hashCode;
            }
        }

        public static RosMessageTypeInfo Create(object rosMessage)
        {
            if (rosMessage == null) throw new ArgumentNullException(nameof(rosMessage));

            return Create(rosMessage.GetType());
        }

        public static RosMessageTypeInfo Create(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            
            throw new NotImplementedException();
        }
    }
}