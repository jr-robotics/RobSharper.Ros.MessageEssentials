using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration;
using Moq;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.Tests
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