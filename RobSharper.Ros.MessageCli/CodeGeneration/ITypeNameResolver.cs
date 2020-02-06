using RobSharper.Ros.MessageParser;

namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    public interface ITypeNameResolver
    {
        string ResolveConcreteTypeName(RosTypeInfo type);
        string ResolveInterfacedTypeName(RosTypeInfo type);
    }
}