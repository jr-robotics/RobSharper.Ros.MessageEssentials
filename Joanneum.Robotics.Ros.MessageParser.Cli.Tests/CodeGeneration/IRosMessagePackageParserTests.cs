using System.Linq;
using FluentAssertions;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration;
using Xunit;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.Tests.CodeGeneration
{
    public abstract class IRosMessagePackageParserTests
    {
        protected abstract IRosMessagePackageParser CreateParser(RosPackageInfo package);
        
        [Fact]
        public void Build_dependency_graph_finds_packages_which_have_to_be_present()
        {
            var package = TestUtils.CreatePackageInfo("common_msgs", "nav_msgs");
            var target = CreateParser(package);
            
            target.ParseMessages();

            target.PackageDependencies.Should().NotBeNull();
            target.PackageDependencies.Should().NotBeEmpty();
            target.PackageDependencies.Should().NotContainNulls();
            
            target.PackageDependencies.Should().Contain("geometry_msgs");
            target.PackageDependencies.Should().Contain("std_msgs");
            target.PackageDependencies.Should().Contain("actionlib_msgs");
            target.PackageDependencies.Count().Should().Be(3);
        }
    }
}