namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public interface ITypeNameResolver
    {
        string ResolveConcreteTypeName(RosTypeInfo type);
        string ResolveInterfacedTypeName(RosTypeInfo type);
    }
}