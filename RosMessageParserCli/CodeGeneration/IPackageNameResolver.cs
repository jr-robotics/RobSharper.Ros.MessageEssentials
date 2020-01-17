namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public interface IPackageNameResolver
    {
        string ResolveTypeName(string rosPackageName, string rosTypeName);
    }
}