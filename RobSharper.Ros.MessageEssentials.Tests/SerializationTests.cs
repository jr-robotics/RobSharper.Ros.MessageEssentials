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
    }
}