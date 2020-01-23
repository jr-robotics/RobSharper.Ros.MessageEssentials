namespace Joanneum.Robotics.Ros.MessageBase.Serialization
{
    public interface IRosMessageFormatter
    {
        bool CanSerialize(IMessageTypeInfo typeInfo, object o);
        void Serialize(SerializationContext context, IMessageTypeInfo messageTypeInfo, object o);
    }
}