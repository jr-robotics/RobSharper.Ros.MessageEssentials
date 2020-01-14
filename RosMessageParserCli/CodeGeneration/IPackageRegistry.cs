namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public interface IPackageRegistry
    {
        PackageRegistryItem AddDependency(string packageName);
    }
}