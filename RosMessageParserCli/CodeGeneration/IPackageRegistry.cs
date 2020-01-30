using System.Collections.Generic;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public interface IPackageRegistry
    {
        IReadOnlyDictionary<string, PackageRegistryItem> Items { get; }
        
        PackageRegistryItem AddDependency(string packageName);
    }
}