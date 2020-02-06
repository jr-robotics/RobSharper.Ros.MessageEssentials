using System.Collections.Generic;

namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    public interface IBuildPackages
    {
        IEnumerable<RosPackageInfo> Packages { get; }
    }
}