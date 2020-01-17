namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public interface ITypeNameResolver
    {
        string ResolveConcreteTypeName(IRosTypeInfo type);
        string ResolveInterfacedTypeName(IRosTypeInfo type);
    }
}