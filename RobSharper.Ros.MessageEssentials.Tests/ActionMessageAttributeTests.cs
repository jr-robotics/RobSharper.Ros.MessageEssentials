using FluentAssertions;
using RobSharper.Ros.MessageEssentials.Tests.RosMessages;
using Xunit;

namespace RobSharper.Ros.MessageEssentials.Tests
{
    public class ActionMessageAttributeTests
    {
        [Fact]
        public void ActionAttributeSetsCorrectMessageTypeNameForGoal()
        {
            var attr = new RosActionMessageAttribute("control_msgs/SingleJointPosition", ActionMessageKind.Goal);

            attr.ServiceName.Should().Be("control_msgs/SingleJointPosition");
            attr.MessageName.Should().Be("control_msgs/SingleJointPositionGoal");
            attr.MessageKind.Should().Be(ActionMessageKind.Goal);
        }
        
        [Fact]
        public void ActionAttributeSetsCorrectMessageTypeNameForResult()
        {
            var attr = new RosActionMessageAttribute("control_msgs/SingleJointPosition", ActionMessageKind.Result);

            attr.ServiceName.Should().Be("control_msgs/SingleJointPosition");
            attr.MessageName.Should().Be("control_msgs/SingleJointPositionResult");
            attr.MessageKind.Should().Be(ActionMessageKind.Result);
        }
        
        [Fact]
        public void ActionAttributeSetsCorrectMessageTypeNameForFeedback()
        {
            var attr = new RosActionMessageAttribute("control_msgs/SingleJointPosition", ActionMessageKind.Feedback);

            attr.ServiceName.Should().Be("control_msgs/SingleJointPosition");
            attr.MessageName.Should().Be("control_msgs/SingleJointPositionFeedback");
            attr.MessageKind.Should().Be(ActionMessageKind.Feedback);
        }

        [Fact]
        public void CanGetRosActionMessageAttributeFromActionMessage()
        {
            var targetType = typeof(SingleJointPositionGoal);
            var attributes = targetType.GetCustomAttributes(typeof(RosActionMessageAttribute), false);

            attributes.Should().HaveCount(1);
            attributes.Should().AllBeAssignableTo<RosMessageBaseAttribute>();
            attributes.Should().AllBeOfType<RosActionMessageAttribute>();
        }
        
        [Fact]
        public void CanGetRosMessageBaseAttributeFromServiceMessage()
        {
            var targetType = typeof(SingleJointPositionGoal);
            var attributes = targetType.GetCustomAttributes(typeof(RosMessageBaseAttribute), false);

            attributes.Should().HaveCount(1);
            attributes.Should().AllBeAssignableTo<RosMessageBaseAttribute>();
            attributes.Should().AllBeOfType<RosActionMessageAttribute>();
        }
    }
}