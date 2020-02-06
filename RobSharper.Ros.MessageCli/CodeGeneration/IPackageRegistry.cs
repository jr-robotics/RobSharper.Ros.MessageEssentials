using System.Collections.Generic;

namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    public interface IPackageRegistry
    {
        IReadOnlyDictionary<string, PackageRegistryItem> Items { get; }
        
        PackageRegistryItem AddDependency(string packageName);
    }
}