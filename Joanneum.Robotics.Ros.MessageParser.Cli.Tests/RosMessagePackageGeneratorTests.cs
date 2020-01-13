using System.Linq;
using System.Reflection;
using FluentAssertions;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration;
using Xunit;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.Tests
{
    public class RosMessagePackageGeneratorTests
    {
        [Fact]
        public void Build_dependency_graph_finds_packages_which_have_to_be_present()
        {
            var target = new RosMessagePackageParser(TestUtils.CreatePackageInfo("common_msgs", "nav_msgs"));
            
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