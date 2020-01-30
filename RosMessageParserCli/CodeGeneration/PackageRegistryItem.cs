using System;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class PackageRegistryItem
    {
        public string PackageName { get; }

        public bool IsInBuildPipeline { get; set; }

        public PackageRegistryItem(string packageName)
        {
            PackageName = packageName ?? throw new ArgumentNullException(nameof(packageName));
        }
    }
}