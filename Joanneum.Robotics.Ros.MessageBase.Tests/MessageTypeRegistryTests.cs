using System;
using FluentAssertions;
using Joanneum.Robotics.Ros.MessageBase.Tests.RosMessages;
using Xunit;

namespace Joanneum.Robotics.Ros.MessageBase.Tests
{
    public class MessageTypeRegistryTests
    {
        [Theory]
        [InlineData(typeof(SimpleInt), SimpleInt.ROS_MD5)]
        [InlineData(typeof(SimpleInt2), SimpleInt2.ROS_MD5)]
        [InlineData(typeof(SimpleIntArray), SimpleIntArray.ROS_MD5)]
        [InlineData(typeof(NestedSimpleInt), NestedSimpleInt.ROS_MD5)]
        [InlineData(typeof(NestedSimpleIntArray), NestedSimpleIntArray.ROS_MD5)]
        public void CreateMessageTypeInfo_with_correct_md5_sum(Type messageType, string expectedMd5Sum)
        {
            var target = new MessageTypeRegistry();
            var typeInfo = target.GetOrCreateMessageTypeInfo(messageType);

            typeInfo.Should().NotBeNull();

            typeInfo.MD5Sum.Should().NotBeNull();
            typeInfo.MD5Sum.Should().Be(expectedMd5Sum);
        }
    }
}