using System;
using FluentAssertions;
using RobSharper.Ros.MessageBase.Tests.RosMessages;
using Xunit;

namespace RobSharper.Ros.MessageBase.Tests
{
    public class MessageTypeInfoTests
    {
        [Theory]
        [InlineData(typeof(SimpleInt), SimpleInt.ROS_MD5)]
        [InlineData(typeof(SimpleInt2), SimpleInt2.ROS_MD5)]
        [InlineData(typeof(SimpleIntArray), SimpleIntArray.ROS_MD5)]
        [InlineData(typeof(NestedSimpleInt), NestedSimpleInt.ROS_MD5)]
        [InlineData(typeof(NestedSimpleIntArray), NestedSimpleIntArray.ROS_MD5)]
        [InlineData(typeof(Empty), Empty.ROS_MD5)]
        [InlineData(typeof(LongConstant), LongConstant.ROS_MD5)]
        [InlineData(typeof(FloatConstant), FloatConstant.ROS_MD5)]
        [InlineData(typeof(BoolConstant), BoolConstant.ROS_MD5)]
        [InlineData(typeof(StringConstant), StringConstant.ROS_MD5)]
        [InlineData(typeof(Combined), Combined.ROS_MD5)]
        [InlineData(typeof(NestedNestedType), NestedNestedType.ROS_MD5)]
        public void CreateMessageTypeInfo_with_correct_md5_sum(Type messageType, string expectedMd5Sum)
        {
            var target = new MessageTypeRegistry();
            var typeInfo = target.GetOrCreateMessageTypeInfo(messageType);

            typeInfo.Should().NotBeNull();

            typeInfo.MD5Sum.Should().NotBeNull();
            typeInfo.MD5Sum.Should().Be(expectedMd5Sum);
        }

        [Theory]
        [InlineData(typeof(SimpleInt), SimpleInt.MESSAGE_DEFINITION)]
        [InlineData(typeof(SimpleInt2), SimpleInt2.MESSAGE_DEFINITION)]
        [InlineData(typeof(SimpleIntArray), SimpleIntArray.MESSAGE_DEFINITION)]
        [InlineData(typeof(NestedSimpleInt), NestedSimpleInt.MESSAGE_DEFINITION)]
        [InlineData(typeof(NestedSimpleIntArray), NestedSimpleIntArray.MESSAGE_DEFINITION)]
        [InlineData(typeof(Empty), Empty.MESSAGE_DEFINITION)]
        [InlineData(typeof(LongConstant), LongConstant.MESSAGE_DEFINITION)]
        [InlineData(typeof(FloatConstant), FloatConstant.MESSAGE_DEFINITION)]
        [InlineData(typeof(BoolConstant), BoolConstant.MESSAGE_DEFINITION)]
        [InlineData(typeof(StringConstant), StringConstant.MESSAGE_DEFINITION)]
        [InlineData(typeof(Combined), Combined.MESSAGE_DEFINITION)]
        public void MessageTypeInfo_has_correct_message_definition(Type messageType, string expectedMessageDefinition)
        {
            expectedMessageDefinition = expectedMessageDefinition?.Replace("\r\n", "\n");
            
            var target = new MessageTypeRegistry();
            var typeInfo = target.GetOrCreateMessageTypeInfo(messageType);

            typeInfo.Should().NotBeNull();

            typeInfo.MessageDefinition.Should().NotBeNull();
            typeInfo.MessageDefinition.Should().Be(expectedMessageDefinition);
        }
    }
}