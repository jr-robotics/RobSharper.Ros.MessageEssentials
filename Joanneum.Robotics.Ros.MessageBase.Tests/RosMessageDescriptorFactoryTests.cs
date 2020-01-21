using System;
using System.ComponentModel;
using FluentAssertions;
using Joanneum.Robotics.Ros.MessageBase.Tests.RosMessages;
using Xunit;

namespace Joanneum.Robotics.Ros.MessageBase.Tests
{
    public class RosMessageDescriptorFactoryTests
    {
        [Fact]
        public void MessageDescriptorFactory_supports_type_with_RosMessageTypeAttribute_generic()
        {
            AttributeBasedRosMessageDescriptorFactory.CanCreate(typeof(Point)).Should().BeTrue();
        }
        
        [Fact]
        public void MessageDescriptorFactory_supports_type_with_RosMessageTypeAttribute()
        {
            AttributeBasedRosMessageDescriptorFactory.CanCreate<Point>().Should().BeTrue();
        }
        
        [Fact]
        public void Can_create_descriptor()
        {
            var messageDescriptor = AttributeBasedRosMessageDescriptorFactory.Create<Point>();

            messageDescriptor.Should().NotBeNull();
        }
    }
}