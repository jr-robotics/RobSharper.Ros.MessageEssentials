using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration;
using Moq;
using Xunit;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.Tests
{
    public class PackageRegistryMessageParserAdapterTests : IRosMessagePackageParserTests
    {
        protected override IRosMessagePackageParser CreateParser(RosPackageInfo package)
        {
            var packageRegistry = CreatePackageRegistryForSinglePackage(package);
            return CreateParser(package, packageRegistry);
        }

        protected PackageRegistryMessageParserAdapter CreateParser(RosPackageInfo package, PackageRegistry packageRegistry)
        {
            var innerParser = new RosMessagePackageParser(package);
            var parser = new PackageRegistryMessageParserAdapter(packageRegistry, innerParser);

            return parser;
        }

        private static PackageRegistry CreatePackageRegistryForSinglePackage(RosPackageInfo package)
        {
            var allPackages = new List<RosPackageInfo>() {package};

            var buildPackagesMock = new Mock<IBuildPackages>();
            buildPackagesMock
                .Setup(x => x.Packages)
                .Returns(allPackages);

            var packageRegistry = new PackageRegistry(buildPackagesMock.Object);
            return packageRegistry;
        }
        
        [Fact]
        public void Parse_messages_adds_dependencies_to_package_registry()
        {
            var package = TestUtils.CreatePackageInfo("common_msgs", "nav_msgs");
            var packageRegistry = CreatePackageRegistryForSinglePackage(package);
            var target = CreateParser(package, packageRegistry);
            
            target.ParseMessages();

            packageRegistry.Items.Should().NotBeNull();
            packageRegistry.Items.Should().NotBeEmpty();

            packageRegistry.Items.Should().ContainKey("geometry_msgs");
            packageRegistry.Items.Should().ContainKey("std_msgs");
            packageRegistry.Items.Should().ContainKey("actionlib_msgs");
            packageRegistry.Items.Count.Should().Be(3);
        }
    }
}