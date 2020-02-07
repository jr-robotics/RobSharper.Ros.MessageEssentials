namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    public interface IRosNamingConvention
    {
        string GetRosTypeName(string rosTypeName, DetailedRosMessageType messageType);
    }
}