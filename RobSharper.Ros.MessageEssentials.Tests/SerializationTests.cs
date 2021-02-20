using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using RobSharper.Ros.MessageEssentials.Serialization;
using RobSharper.Ros.MessageEssentials.Tests.RosMessages;
using Xunit;

namespace RobSharper.Ros.MessageEssentials.Tests
{
    public class SerializationTests
    {
        public static IEnumerable<object[]> TestMessages = new List<object[]>()
        {
            new object[]
            {
                new SimpleInt
                {
                    A = 537
                }
            },
            
            new object[]
            {
                new NestedSimpleInt()
                {
                    A = new SimpleInt() { A = 777 }
                }
            },
            
            new object[]
            {
                new SimpleIntArray()
                {
                    A = new List<int>
                    {
                        1,
                        2,
                        3
                    }
                }
            },
            
            new object[]
            {
                new NestedSimpleIntArray()
                {
                    A = new List<SimpleInt>
                    {
                        new SimpleInt() { A = 15 },
                        new SimpleInt() { A = 22 },
                        new SimpleInt() { A = -487 }
                    }
                }
            },
            
            new object[]
            {
                new Combined()
                {
                    A = 15,
                    B = 44
                }
            },
            
            new object[]
            {
                new EnumMessage()
                {
                    A = EnumMessage.EnumValues.B,
                    B = EnumMessage.EnumValues.B
                }
            },
            
            new object[]
            {
                new EnumMessage()
                {
                    A = EnumMessage.EnumValues.A,
                    B = EnumMessage.EnumValues.A
                }
            },
            
            new object[]
            {
                new EnumGoalStatus()
                {
                    GoalId = 37,
                    Status = GoalStatusValue.Recalling,
                    Text = "Test"
                }
            },
            
            new object[]
            {
                new GoalStatus()
                {
                    GoalId = 37,
                    Status = 4,
                    Text = "test"
                }
            },
            
            new object[]
            {
                new SimpleIntField()
                {
                    A = 555
                }
            }
            
            // Null is not supported. Should this be the case?
            // new object[]
            // {
            //     new NestedSimpleInt() { A = null} 
            // }
        };
        
        [Theory]
        [MemberData(nameof(TestMessages))]
        public void CanSerialize(object message)
        {
            var typeRegistry = new MessageTypeRegistry();
            var serializer = new RosMessageSerializer(typeRegistry);
            var ms = new MemoryStream();

            serializer.Serialize(message, ms);

            ms.ToArray().Should().NotBeNull();

            ms.Position = 0;
            var deserializedMessage = serializer.Deserialize(message.GetType(), ms);

            deserializedMessage.Should().NotBeNull();
            deserializedMessage.Should().NotBeSameAs(message);
            deserializedMessage.Should().BeEquivalentTo(message);
        }
        
        [Fact]
        public void CanSerializeEmptyMessage()
        {
            var typeRegistry = new MessageTypeRegistry();
            var serializer = new RosMessageSerializer(typeRegistry);
            var ms = new MemoryStream();
            var message = new Empty();

            serializer.Serialize(message, ms);

            ms.ToArray().Should().NotBeNull();

            ms.Position = 0;
            var deserializedMessage = serializer.Deserialize(message.GetType(), ms);

            deserializedMessage.Should().NotBeNull();
            deserializedMessage.Should().NotBeSameAs(message);
        }

        [Fact]
        public void CanSerializeEnumMessageAndDeserializeWithoutEnum()
        {
            var typeRegistry = new MessageTypeRegistry();
            var serializer = new RosMessageSerializer(typeRegistry);
            var ms = new MemoryStream();

            const int expectedGoalId = 17;
            const byte expectedStatus = 6;
            const string expectedText = "test";

            var message = new EnumGoalStatus
            {
                GoalId = expectedGoalId,
                Status = (GoalStatusValue) expectedStatus,
                Text = expectedText
            };

            serializer.Serialize(message, ms);

            ms.ToArray().Should().NotBeNull();

            ms.Position = 0;
            var deserializedMessage = (GoalStatus) serializer.Deserialize(typeof(GoalStatus), ms);

            deserializedMessage.Should().NotBeNull();

            deserializedMessage.GoalId.Should().Be(expectedGoalId);
            deserializedMessage.Status.Should().Be(expectedStatus);
            deserializedMessage.Text.Should().Be(expectedText);
        }

        [Fact]
        public void CanSerializeEquivalentIntMessage()
        {
            var typeRegistry = new MessageTypeRegistry();
            var serializer = new RosMessageSerializer(typeRegistry);
            var ms = new MemoryStream();

            const int expectedValue = 17;
            const string expectedText = "It could be any string";

            var message = new EquivalentIntMessages.IntMessage()
            {
                Value = expectedValue,
                Text = expectedText
            };

            serializer.Serialize(message, ms);

            ms.ToArray().Should().NotBeNull();

            ms.Position = 0;
            var deserializedMessage = (EquivalentIntMessages.ShortMessage) serializer.Deserialize(typeof(EquivalentIntMessages.ShortMessage), ms);

            deserializedMessage.Should().NotBeNull();
            deserializedMessage.Value.Should().Be(expectedValue);
            deserializedMessage.Text.Should().Be(expectedText);
        }

        [Fact]
        public void CanSerializeEquivalentIntMessageReverse()
        {
            var typeRegistry = new MessageTypeRegistry();
            var serializer = new RosMessageSerializer(typeRegistry);
            var ms = new MemoryStream();

            const int expectedValue = 17;
            const string expectedText = "It could be any string";

            var message = new EquivalentIntMessages.ShortMessage()
            {
                Value = expectedValue,
                Text = expectedText
            };

            serializer.Serialize(message, ms);

            ms.ToArray().Should().NotBeNull();

            ms.Position = 0;
            var deserializedMessage = (EquivalentIntMessages.IntMessage) serializer.Deserialize(typeof(EquivalentIntMessages.IntMessage), ms);

            deserializedMessage.Should().NotBeNull();
            deserializedMessage.Value.Should().Be(expectedValue);
            deserializedMessage.Text.Should().Be(expectedText);
        }
    }
}