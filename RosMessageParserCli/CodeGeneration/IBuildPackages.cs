using System.Collections.Generic;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public interface IBuildPackages
    {
        IEnumerable<RosPackageInfo> Packages { get; }
    }
}