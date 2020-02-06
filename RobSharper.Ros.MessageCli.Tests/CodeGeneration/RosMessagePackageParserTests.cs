using Moq;
using RobSharper.Ros.MessageCli.CodeGeneration;
using RobSharper.Ros.MessageCli.CodeGeneration.MessagePackage;

namespace RobSharper.Ros.MessageCli.Tests.CodeGeneration
{
    public class RosMessagePackageParserTests : IRosMessagePackageParserTests
    {
        private static IBuildPackages CreateBuildPackages(params RosPackageInfo[] buildPackages)
        {
            var buildPackagesMock = new Mock<IBuildPackages>();
            buildPackagesMock
                .Setup(x => x.Packages)
                .Returns(buildPackages);

            return buildPackagesMock.Object;
        }
        
        protected override IRosMessagePackageParser CreateParser(RosPackageInfo package)
        {
            return new RosMessagePackageParser(package, CreateBuildPackages(package));
        }
    }
}