using System.IO;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.Tests
{
    public class TestUtils
    {
        public static RosPackageInfo CreatePackageInfo(params string[] packageNames)
        {
            var packageFolder = Path.Combine("TestPackages", Path.Combine(packageNames));
            var package = RosPackageInfo.Create(packageFolder);

            return package;
        }
    }
}