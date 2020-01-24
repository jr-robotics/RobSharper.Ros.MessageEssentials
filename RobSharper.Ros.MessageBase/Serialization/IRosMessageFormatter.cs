namespace RobSharper.Ros.MessageBase.Serialization
{
    public interface IRosMessageFormatter
    {
        bool CanSerialize(IMessageTypeInfo typeInfo);
        
        void Serialize(SerializationContext context, RosBinaryWriter writer, IMessageTypeInfo messageTypeInfo, object o);
        
        object Deserialize(SerializationContext context, RosBinaryReader writer, IMessageTypeInfo messageTypeInfo);
    }
}