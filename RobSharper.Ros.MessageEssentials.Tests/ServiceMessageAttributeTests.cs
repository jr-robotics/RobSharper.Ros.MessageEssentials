using FluentAssertions;
using RobSharper.Ros.MessageEssentials.Tests.RosMessages;
using Xunit;

namespace RobSharper.Ros.MessageEssentials.Tests
{
    public class ServiceMessageAttributeTests
    {
        [Fact]
        public void RequestAttributeSetsCorrectMessageTypeNameForRequest()
        {
            var attr = new RosServiceMessageAttribute("std_srvs/SetBool", ServiceMessageKind.Request);

            attr.ServiceName.Should().Be("std_srvs/SetBool");
            attr.MessageName.Should().Be("std_srvs/SetBoolRequest");
            attr.MessageKind.Should().Be(ServiceMessageKind.Request);
        }
        
        [Fact]
        public void RequestAttributeSetsCorrectMessageTypeNameForResponse()
        {
            var attr = new RosServiceMessageAttribute("std_srvs/SetBool", ServiceMessageKind.Response);

            attr.ServiceName.Should().Be("std_srvs/SetBool");
            attr.MessageName.Should().Be("std_srvs/SetBoolResponse");
            attr.MessageKind.Should().Be(ServiceMessageKind.Response);
        }

        [Fact]
        public void CanGetRosServiceMessageAttributeFromServiceMessage()
        {
            var targetType = typeof(SetBoolRequest);
            var attributes = targetType.GetCustomAttributes(typeof(RosServiceMessageAttribute), false);

            attributes.Should().HaveCount(1);
            attributes.Should().AllBeAssignableTo<RosMessageBaseAttribute>();
            attributes.Should().AllBeOfType<RosServiceMessageAttribute>();
        }
        
        [Fact]
        public void CanGetRosMessageAttributeFromServiceMessage()
        {
            var targetType = typeof(SetBoolRequest);
            var attributes = targetType.GetCustomAttributes(typeof(RosMessageBaseAttribute), false);

            attributes.Should().HaveCount(1);
            attributes.Should().AllBeAssignableTo<RosMessageBaseAttribute>();
            attributes.Should().AllBeOfType<RosServiceMessageAttribute>();
        }
    }
}