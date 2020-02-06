using System.IO;
using RobSharper.Ros.MessageCli.CodeGeneration;

namespace RobSharper.Ros.MessageCli.Tests.CodeGeneration
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