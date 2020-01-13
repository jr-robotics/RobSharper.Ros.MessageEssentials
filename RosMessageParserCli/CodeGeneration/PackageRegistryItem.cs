using System;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class PackageRegistryItem
    {
        public string PackageName { get; }

        public bool IsAvailable { get; private set; }
        public bool IsInBuildPipeline { get; private set; }

        public PackageRegistryItem(string packageName)
        {
            PackageName = packageName ?? throw new ArgumentNullException(nameof(packageName));
        }

        public void SetIsInBuildPipline()
        {
            IsInBuildPipeline = true;
            IsAvailable = true;
        }
    }
}