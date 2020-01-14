using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.Tests
{
    public class RosMessagePackageParserTests : IRosMessagePackageParserTests
    {
        protected override IRosMessagePackageParser CreateParser(RosPackageInfo package)
        {
            return new RosMessagePackageParser(package);
        }
    }
}