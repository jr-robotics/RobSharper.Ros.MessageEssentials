namespace RobSharper.Ros.MessageBase.Serialization
{
    public interface IRosMessageFormatter
    {
        bool CanSerialize(IMessageTypeInfo typeInfo);
        void Serialize(SerializationContext context, IMessageTypeInfo messageTypeInfo, object o);
        object Deserialize(SerializationContext context, IMessageTypeInfo messageTypeInfo);
    }
}