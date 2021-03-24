using System;
using System.Linq;
using FluentAssertions;
using RobSharper.Ros.MessageEssentials.Serialization;
using Xunit;

namespace RobSharper.Ros.MessageEssentials.Tests.Serialization
{
    public class RosFieldSerializationExceptionTests
    {
        [Theory]
        [InlineData(new object[] { new [] {"item"}})]
        [InlineData(new object[] { new [] {"item", "value"}})]
        [InlineData(new object[] { new [] {"a", "b", "c"}})]
        [InlineData(new object[] { new [] {"item", "[5]"}})]
        [InlineData(new object[] { new [] {"item", "values", "[5]"}})]
        public void Can_get_formatted_identifier(string[] identifierSlugs)
        {
            var target = CreateRosFieldSerializationException(identifierSlugs);
            var expectedIdentifier = CreateExpectedIdentifier(identifierSlugs);
            
            target.Identifier.Should().NotBeNullOrEmpty();
            target.Identifier.Should().Be(expectedIdentifier);
        }
        
        [Theory]
        [InlineData(RosFieldSerializationException.SerializationOperation.Serialize, new [] {"item"})]
        [InlineData(RosFieldSerializationException.SerializationOperation.Serialize, new [] {"item", "value"})]
        [InlineData(RosFieldSerializationException.SerializationOperation.Serialize, new [] {"a", "b", "c"})]
        [InlineData(RosFieldSerializationException.SerializationOperation.Serialize, new [] {"item", "[5]"})]
        [InlineData(RosFieldSerializationException.SerializationOperation.Serialize, new [] {"item", "values", "[5]"})]
        [InlineData(RosFieldSerializationException.SerializationOperation.Deserialize, new [] {"item"})]
        [InlineData(RosFieldSerializationException.SerializationOperation.Deserialize, new [] {"item", "value"})]
        [InlineData(RosFieldSerializationException.SerializationOperation.Deserialize, new [] {"a", "b", "c"})]
        [InlineData(RosFieldSerializationException.SerializationOperation.Deserialize, new [] {"item", "[5]"})]
        [InlineData(RosFieldSerializationException.SerializationOperation.Deserialize, new [] {"item", "values", "[5]"})]
        public void Can_get_formatted_message(RosFieldSerializationException.SerializationOperation op, string[] identifierSlugs)
        {
            var target = CreateRosFieldSerializationException(identifierSlugs, null, op);
            var expectedIdentifier = CreateExpectedIdentifier(identifierSlugs);
            var expectedVerb = "";

            switch (op)
            {
                case RosFieldSerializationException.SerializationOperation.Serialize:
                    expectedVerb = "serialize";
                    break;
                case RosFieldSerializationException.SerializationOperation.Deserialize:
                    expectedVerb = "deserialize";
                    break;
            }
            
            target.Message.Should().NotBeNullOrEmpty();
            target.Message.Should().Contain(expectedIdentifier);
            target.Message.Should().Contain(expectedVerb);
        }

        [Fact]
        public void Message_should_contain_inner_exception_message()
        {
            var innerException = new Exception("This is the inner exception");
            var target =
                new RosFieldSerializationException(RosFieldSerializationException.SerializationOperation.Serialize,
                    "item", innerException);

            target.Message.Should().Contain(innerException.Message);
        }

        private static RosFieldSerializationException CreateRosFieldSerializationException(string[] identifierSlugs,
            Exception innerException = null,
            RosFieldSerializationException.SerializationOperation op =
                RosFieldSerializationException.SerializationOperation.Serialize)
        {
            var reversedSlugs = identifierSlugs.Reverse();

            var target = new RosFieldSerializationException(op,
                reversedSlugs.FirstOrDefault(), innerException);

            foreach (var slug in reversedSlugs.Skip(1))
            {
                target.AddLeadingRosIdentifier(slug);
            }

            return target;
        }

        private static string CreateExpectedIdentifier(string[] identifierSlugs)
        {
            var expectedIdentifier = string.Join('.', identifierSlugs);
            expectedIdentifier = expectedIdentifier.Replace(".[", "[");
            return expectedIdentifier;
        }
    }
}