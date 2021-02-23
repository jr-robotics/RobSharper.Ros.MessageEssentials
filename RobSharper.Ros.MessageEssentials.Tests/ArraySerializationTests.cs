using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using RobSharper.Ros.MessageEssentials.Serialization;
using Xunit;

namespace RobSharper.Ros.MessageEssentials.Tests
{
    public class ArraySerializationTests
    {
        [Fact(Skip = "Arrays are not supported yet")]
        public void CanSerializeArrayMessage()
        {
            Test(new IntArrayMessage());
        }
        
        [Fact]
        public void CanSerializeIListMessage()
        {
            Test(new IntIListMessage());
        }
        
        [Fact]
        public void CanSerializeICollectionMessage()
        {
            Test(new IntICollectionMessage());
        }
        
        [Fact]
        public void CanSerializeIEnumerableMessage()
        {
            Test(new IntIEnumerableMessage());
        }
        
        private void Test<TMessage>(TMessage message) where TMessage : IIntValues
        {
            var serializer = new RosMessageSerializer(new MessageTypeRegistry());
            var ms = new MemoryStream();

            int[] expectedValues = {1, 2, 3, 4, 5};
            int expectedSum = 15;

            message.Values = expectedValues;

            serializer.Serialize(message, ms);
            
            ms.ToArray().Should().NotBeNull();
            ms.Position = 0;
            
            var deserializedMessage = (TMessage) serializer.Deserialize(typeof(TMessage), ms);

            deserializedMessage.Should().NotBeNull();
            deserializedMessage.Values.All(x => x > 0).Should().BeTrue("all items should be > 0");
            deserializedMessage.Values.Sum().Should().Be(expectedSum, "the sum of all items should be 15");
        }

        private interface IIntValues
        {
            IEnumerable<int> Values { get; set; }
        }
        
        [RosMessage("test_msgs/IntegerArray")]
        public class IntArrayMessage : IIntValues
        {
            [RosMessageField("int32[]", "values", 1)]
            public int[] Values { get; set; }

            IEnumerable<int> IIntValues.Values
            {
                get => Values;
                set => Values = value?.ToArray();
            }
        }
    
        [RosMessage("test_msgs/IntegerIList")]
        public class IntIListMessage : IIntValues
        {
            [RosMessageField("int32[]", "values", 1)]
            public IList<int> Values { get; set; }

            IEnumerable<int> IIntValues.Values
            {
                get => Values;
                set => Values = value?.ToList();
            }
        }
    
        [RosMessage("test_msgs/IntegerICollection")]
        public class IntICollectionMessage : IIntValues
        {
            [RosMessageField("int32[]", "values", 1)]
            public ICollection<int> Values { get; set; }

            IEnumerable<int> IIntValues.Values
            {
                get => Values;
                set => Values = value?.ToList();
            }
        }
    
        [RosMessage("test_msgs/IntegerIEnumerable")]
        public class IntIEnumerableMessage : IIntValues
        {
            [RosMessageField("int32[]", "values", 1)]
            public IEnumerable<int> Values { get; set; }
        }
    }
}