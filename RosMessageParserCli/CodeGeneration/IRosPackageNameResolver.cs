namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public interface IRosPackageNameResolver
    {
        string ResolvePackageName(string rosPackageName);
    }
}