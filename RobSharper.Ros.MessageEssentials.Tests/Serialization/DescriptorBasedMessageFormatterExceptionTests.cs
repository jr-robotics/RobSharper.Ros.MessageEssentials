using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using RobSharper.Ros.MessageEssentials.Serialization;
using Xunit;

namespace RobSharper.Ros.MessageEssentials.Tests.Serialization
{
    public class DescriptorBasedMessageFormatterExceptionTests
    {
        [RosMessage("test_msgs/String")]
        public class StringMsg
        {
            [RosMessageField("string", "value", 1)]
            public string Value { get; set; } = string.Empty;
        }
        
        [RosMessage("test_msgs/NestedString")]
        public class NestedStringMsg
        {
            [RosMessageField("test_msgs/String", "item", 1)]
            public StringMsg Item { get; set; } = new StringMsg();
        }
        
        [RosMessage("test_msgs/VariableStringArray")]
        public class VariableStringArrayMsg
        {
            [RosMessageField("test_msgs/String[]", "items", 1)]
            public ICollection<StringMsg> Items { get; set; } = new List<StringMsg>();
        }
        
        [RosMessage("test_msgs/FixedSizeStringArray")]
        public class FixedSizeStringArrayMsg
        {
            [RosMessageField("test_msgs/String[3]", "items", 1)]
            public ICollection<StringMsg> Items { get; set; } = new List<StringMsg>();

            public FixedSizeStringArrayMsg()
            {
                Items.PopulateWithInitializedRosValues(3);
            }
        }

        private static Action Serialize(object message)
        {
            return () =>
            {
                var formatter = new DescriptorBasedMessageFormatter();
                var context = new SerializationContext(Stream.Null, new[] {formatter}, new MessageTypeRegistry());
                var messageTypeInfo = context.MessageTypeRegistry.GetOrCreateMessageTypeInfo(message.GetType());
                var writer = new RosBinaryWriter(context.Stream);

                formatter.Serialize(context, writer, messageTypeInfo, message);
            };
        }

        public class StringMessage
        {
            [Fact]
            public void Serialization_does_not_throw_with_empty_value()
            {
                var message = new StringMsg
                {
                    Value = string.Empty
                };

                Serialize(message).Should()
                    .NotThrow();
            }
            
            [Fact]
            public void Serialization_does_not_throw_with_value()
            {
                var message = new StringMsg
                {
                    Value = "Test message"
                };

                Serialize(message).Should()
                    .NotThrow();
            }
            
            [Fact]
            public void Serialization_fails_with_null_value()
            {
                var message = new StringMsg
                {
                    Value = null
                };

                Serialize(message).Should()
                    .Throw<RosFieldSerializationException>()
                    .Where(x => x.Identifier == "value");
            }
        }

        public class NestedStringMessage
        {
            [Fact]
            public void Serialization_does_not_throw_with_empty_item_value()
            {
                var message = new NestedStringMsg
                {
                    Item = new StringMsg
                    {
                        Value = string.Empty
                    }
                };

                Serialize(message).Should()
                    .NotThrow();
            }
            
            [Fact]
            public void Serialization_does_not_throw_with_item_value()
            {
                var message = new NestedStringMsg
                {
                    Item = new StringMsg
                    {
                        Value = "Test message"
                    }
                };

                Serialize(message).Should()
                    .NotThrow();
            }
            
            [Fact]
            public void Serialization_fails_with_null_item_value()
            {
                var message = new NestedStringMsg
                {
                    Item = new StringMsg
                    {
                        Value = null
                    }
                };

                Serialize(message).Should()
                    .Throw<RosFieldSerializationException>()
                    .Where(x => x.Identifier == "item.value");
            }
            
            [Fact]
            public void Serialization_fails_with_null_item()
            {
                var message = new NestedStringMsg
                {
                    Item = null
                };

                Serialize(message).Should()
                    .Throw<RosFieldSerializationException>()
                    .Where(x => x.Identifier == "item");
            }
        }

        public class VariableSizeStringArray
        {
            [Fact]
            public void Serialization_does_not_throw_with_empty_items()
            {
                var message = new VariableStringArrayMsg()
                {
                    Items = new List<StringMsg>()
                };

                Serialize(message).Should()
                    .NotThrow();
            }
            
            [Fact]
            public void Serialization_does_not_throw_with_valid_items()
            {
                var message = new VariableStringArrayMsg()
                {
                    Items = new List<StringMsg>
                    {
                        new StringMsg { Value = "Item 1" },
                        new StringMsg { Value = "Item 2" },
                        new StringMsg { Value = "Item 3" }
                    }
                };

                Serialize(message).Should()
                    .NotThrow();
            }
            
            [Fact]
            public void Serialization_fails_with_null_item_value()
            {
                var message = new VariableStringArrayMsg()
                {
                    Items = new List<StringMsg>
                    {
                        new StringMsg { Value = "Item 1" },
                        new StringMsg { Value = null },
                        new StringMsg { Value = "Item 3" }
                    }
                };

                Serialize(message).Should()
                    .Throw<RosFieldSerializationException>()
                    .Where(x => x.Identifier == "items[1].value");
            }
            
            [Fact]
            public void Serialization_fails_with_null_items()
            {
                var message = new VariableStringArrayMsg()
                {
                    Items = null
                };

                Serialize(message).Should()
                    .Throw<RosFieldSerializationException>()
                    .Where(x => x.Identifier == "items");
            }
        }

        public class FixedSizeStringArray
        {
            [Fact]
            public void Serialization_fails_with_empty_items()
            {
                var message = new FixedSizeStringArrayMsg()
                {
                    Items = new List<StringMsg>()
                };

                Serialize(message).Should()
                    .Throw<RosFieldSerializationException>()
                    .Where(x => x.Identifier == "items");
            }
            
            [Fact]
            public void Serialization_does_not_throw_with_valid_items()
            {
                var message = new FixedSizeStringArrayMsg()
                {
                    Items = new List<StringMsg>
                    {
                        new StringMsg { Value = "Item 1" },
                        new StringMsg { Value = "Item 2" },
                        new StringMsg { Value = "Item 3" }
                    }
                };

                Serialize(message).Should()
                    .NotThrow();
            }
            [Fact]
            public void Serialization_fails_with_wrong_number_of_items()
            {
                var message = new FixedSizeStringArrayMsg()
                {
                    Items = new List<StringMsg>
                    {
                        new StringMsg { Value = "Item 1" },
                        new StringMsg { Value = "Item 2" }
                    }
                };

                Serialize(message).Should()
                    .Throw<RosFieldSerializationException>()
                    .Where(x => x.Identifier == "items");
            }
            
            [Fact]
            public void Serialization_fails_with_null_item_value()
            {
                var message = new FixedSizeStringArrayMsg()
                {
                    Items = new List<StringMsg>
                    {
                        new StringMsg { Value = "Item 1" },
                        new StringMsg { Value = null },
                        new StringMsg { Value = "Item 3" }
                    }
                };

                Serialize(message).Should()
                    .Throw<RosFieldSerializationException>()
                    .Where(x => x.Identifier == "items[1].value");
            }
            
            [Fact]
            public void Serialization_fails_with_null_items()
            {
                var message = new FixedSizeStringArrayMsg()
                {
                    Items = null
                };

                Serialize(message).Should()
                    .Throw<RosFieldSerializationException>()
                    .Where(x => x.Identifier == "items");
            }
        }
    }
}