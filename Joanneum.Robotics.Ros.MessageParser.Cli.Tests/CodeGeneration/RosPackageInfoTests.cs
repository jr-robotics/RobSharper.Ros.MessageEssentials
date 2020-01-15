using System.IO;
using System.Linq;
using FluentAssertions;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration;
using Xunit;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.Tests.CodeGeneration
{
    public class RosPackageInfoTests
    {
        [Fact]
        public void HasMessages_returns_true_if_package_contains_msg_file()
        {
            var packageFolder = Path.Combine("TestPackages", "msg_msgs");
            var package = RosPackageInfo.Create(packageFolder);

            package.HasMessages.Should().BeTrue();
            package.Messages.Should().NotBeNull();
            package.Messages.Should().NotBeEmpty();
            package.Messages.Should().OnlyContain(f => f.GetRosMessageType() == RosMessageType.Message);
        }
        
        [Fact]
        public void HasMessages_returns_true_if_package_contains_srv_file()
        {
            var packageFolder = Path.Combine("TestPackages", "srv_msgs");
            var package = RosPackageInfo.Create(packageFolder);

            package.HasMessages.Should().BeTrue();
            package.Messages.Should().NotBeNull();
            package.Messages.Should().NotBeEmpty();
            package.Messages.Should().OnlyContain(f => f.GetRosMessageType() == RosMessageType.Service);
        }
        
        [Fact]
        public void HasMessages_returns_true_if_package_contains_action_file()
        {
            var packageFolder = Path.Combine("TestPackages", "action_msgs");
            var package = RosPackageInfo.Create(packageFolder);

            package.HasMessages.Should().BeTrue();
            package.Messages.Should().NotBeNull();
            package.Messages.Should().NotBeEmpty();
            package.Messages.Should().OnlyContain(f => f.GetRosMessageType() == RosMessageType.Action);
        }

        [Fact]
        void MessageFiles_returns_list_of_message_files()
        {
            var packageFolder = Path.Combine("TestPackages", "std_msgs");
            var package = RosPackageInfo.Create(packageFolder);

            package.Should().NotBeNull();

            package.Messages.Should().NotBeNull();
            package.Messages.Count().Should().BeGreaterThan(0);
            package.Messages.Should().OnlyContain(f => f.GetRosMessageType() != RosMessageType.None);
        }
        
    }
}