namespace RobSharper.Ros.MessageBase.Serialization
{
    public interface IRosMessageFormatter
    {
        bool CanSerialize(IRosMessageTypeInfo typeInfo);
        
        void Serialize(SerializationContext context, RosBinaryWriter writer, IRosMessageTypeInfo messageTypeInfo, object o);
        
        object Deserialize(SerializationContext context, RosBinaryReader reader, IRosMessageTypeInfo messageTypeInfo);
    }
}