namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    public interface IResourceNamingConvention
    {
        string GetNamespace(string rosPackageName);
        string GetTypeName(string rosTypeName);
        string GetTypeName(string rosTypeName, DetailedRosMessageType messageType);
    }
}