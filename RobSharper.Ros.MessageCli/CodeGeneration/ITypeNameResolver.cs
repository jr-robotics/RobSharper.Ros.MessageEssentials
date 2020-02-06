using RobSharper.Ros.MessageParser;

namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    public interface ITypeNameResolver
    {
        string ResolveFullQualifiedTypeName(RosTypeInfo type);
        string ResolveFullQualifiedInterfaceName(RosTypeInfo type);
    }
}