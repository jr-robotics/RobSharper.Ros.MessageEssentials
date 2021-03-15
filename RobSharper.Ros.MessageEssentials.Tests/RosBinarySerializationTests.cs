using System;
using System.IO;
using FluentAssertions;
using RobSharper.Ros.MessageEssentials.Serialization;
using Xunit;

namespace RobSharper.Ros.MessageEssentials.Tests
{
    public class RosBinarySerializationTests
    {
        [Fact]
        public void CanWriteAndReadInt()
        {
            using (var s = new MemoryStream())
            {
                var writer = new RosBinaryWriter(s);
                var reader = new RosBinaryReader(s);

                writer.Write((int) 4);
                writer.Write((int) 5);
                writer.Write((int) 6);

                s.Position = 0;

                reader.ReadInt32().Should().Be(4);
                reader.ReadInt32().Should().Be(5);
                reader.ReadInt32().Should().Be(6);
            }
        }
        
        [Fact]
        public void CanWriteAndReadString()
        {
            using (var s = new MemoryStream())
            {
                var writer = new RosBinaryWriter(s);
                var reader = new RosBinaryReader(s);

                writer.Write("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxASDF");

                s.Position = 0;

                reader.ReadString().Should().Be("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxASDF");
            }
        }

        [Fact]
        public void CanWriteAndReadDateTime()
        {
            using (var s = new MemoryStream())
            {
                var writer = new RosBinaryWriter(s);
                var reader = new RosBinaryReader(s);
                var writtenTime = DateTime.Now;
                
                // Writing should convert to UTC.
                writer.WriteBuiltInType(typeof(DateTime), writtenTime);
                
                // Read back what we wrote.
                s.Position = 0;
                var readTime = (DateTime)reader.ReadBuiltInType(typeof(DateTime));
                
                // Expected behaviour is that time in ROS-Messages is UTC.
                // Max difference is 0.001 since only milliseconds are preserved and ROS uses nanoseconds.
                Assert.Equal(writtenTime.ToUniversalTime(), readTime, TimeSpan.FromSeconds(0.001));
            }
        }

        [Fact]
        public void CanWriteAndReadLocalDateTime()
        {
            using (var s = new MemoryStream())
            {
                var writer = new RosBinaryWriter(s);
                var reader = new RosBinaryReader(s);
                var writtenTime = new DateTime(2021, 03, 15, 18, 55 ,12, 7, DateTimeKind.Local);
                
                // Writing should convert to UTC.
                writer.WriteBuiltInType(typeof(DateTime), writtenTime);
                
                // Read back what we wrote.
                s.Position = 0;
                var readTime = (DateTime)reader.ReadBuiltInType(typeof(DateTime));
                
                // Expected behaviour is that time in ROS-Messages is UTC.
                // Max difference is 0.001 since only milliseconds are preserved and ROS uses nanoseconds.
                readTime.Kind.Should().Be(DateTimeKind.Utc);
                readTime.Should().BeCloseTo(writtenTime.ToUniversalTime(), TimeSpan.FromSeconds(0.001));
            }
        }

        [Fact]
        public void CanWriteAndReadUtcDateTime()
        {
            using (var s = new MemoryStream())
            {
                var writer = new RosBinaryWriter(s);
                var reader = new RosBinaryReader(s);
                var writtenTime = new DateTime(2021, 03, 15, 18, 55 ,12, 7, DateTimeKind.Utc);
                
                // Writing should convert to UTC.
                writer.WriteBuiltInType(typeof(DateTime), writtenTime);
                
                // Read back what we wrote.
                s.Position = 0;
                var readTime = (DateTime)reader.ReadBuiltInType(typeof(DateTime));
                
                // Expected behaviour is that time in ROS-Messages is UTC.
                // Max difference is 0.001 since only milliseconds are preserved and ROS uses nanoseconds.
                readTime.Kind.Should().Be(DateTimeKind.Utc);
                readTime.Should().BeCloseTo(writtenTime, TimeSpan.FromSeconds(0.001));
            }
        }

        [Fact]
        public void CanWriteAndReadUnspecifiedDateTime()
        {
            using (var s = new MemoryStream())
            {
                var writer = new RosBinaryWriter(s);
                var reader = new RosBinaryReader(s);
                var writtenTime = new DateTime(2021, 03, 15, 18, 55 ,12, 7, DateTimeKind.Unspecified);
                
                // Writing should convert to UTC.
                writer.WriteBuiltInType(typeof(DateTime), writtenTime);
                
                // Read back what we wrote.
                s.Position = 0;
                var readTime = (DateTime)reader.ReadBuiltInType(typeof(DateTime));
                
                // Expected behaviour is that time in ROS-Messages is UTC.
                // Max difference is 0.001 since only milliseconds are preserved and ROS uses nanoseconds.
                readTime.Kind.Should().Be(DateTimeKind.Utc);
                readTime.Should().BeCloseTo(writtenTime, TimeSpan.FromSeconds(0.001));
            }
        }

        [Fact]
        public void CanSerializeDateTimeBefore1970()
        {
            using (var s = new MemoryStream())
            {
                var writer = new RosBinaryWriter(s);
                var reader = new RosBinaryReader(s);
                var writtenTime = new DateTime(1964, 01, 15, 18, 55 ,12, 7, DateTimeKind.Unspecified);
                
                // Writing should convert to UTC.
                writer.WriteBuiltInType(typeof(DateTime), writtenTime);
                
                // Read back what we wrote.
                s.Position = 0;
                var readTime = (DateTime)reader.ReadBuiltInType(typeof(DateTime));
                
                // Expected behaviour is that time in ROS-Messages is UTC.
                // Max difference is 0.001 since only milliseconds are preserved and ROS uses nanoseconds.
                readTime.Kind.Should().Be(DateTimeKind.Utc);
                readTime.Should().BeCloseTo(writtenTime, TimeSpan.FromSeconds(0.001));
            }
        }
        
        [Fact]
        public void CanWriteAndReadTimeSpan()
        {
            using (var s = new MemoryStream())
            {
                var writer = new RosBinaryWriter(s);
                var reader = new RosBinaryReader(s);
                var writtenTimeSpan = TimeSpan.FromSeconds(123456789.123);
                
                writer.WriteBuiltInType(typeof(TimeSpan), writtenTimeSpan);
                
                s.Position = 0;
                var readTime = (TimeSpan)reader.ReadBuiltInType(typeof(TimeSpan));
                
                Assert.Equal(writtenTimeSpan, readTime);
            }
        }

        private enum EnumValues
        {
            AValue = 1,
            BValue = 2,
            CValue = 3
        }
        
        [Theory]
        [InlineData("int32", (int)10, (int)10)]
        [InlineData("int32", (short)10, (short)10)]
        [InlineData("int32", (int)10, (short)10)]
        [InlineData("int32", (short)10, (int)10)]
        [InlineData("int32", (long)10, (long)10)]
        [InlineData("int32", (int)10, (long)10)]
        [InlineData("int32", (long)10, (int)10)]
        [InlineData("int32", (byte)10, (byte)10)]
        [InlineData("int32", (int)10, (byte)10)]
        [InlineData("int32", (byte)10, (int)10)]
        [InlineData("int32", (uint)10, (uint)10)]
        [InlineData("int32", (uint)10, (int)10)]
        [InlineData("int32", (int)10, (uint)10)]
        [InlineData("int8", (sbyte)10, (int)10)]
        [InlineData("uint8", (byte)10, (int)10)]
        [InlineData("byte", (byte)10, (byte)10)]
        [InlineData("char", (int)10, (int)10)]
        [InlineData("uint8", EnumValues.AValue, EnumValues.AValue)]
        [InlineData("uint8", 2, EnumValues.BValue)]
        [InlineData("uint8", EnumValues.CValue, 3)]
        [InlineData("string", EnumValues.CValue, "CValue")]
        [InlineData("string", "CValue", EnumValues.CValue)]
        [InlineData("string", "", "")]
        public void CanWriteAndReadBuiltInTypes2(string rosType, object writeValue, object expectedValue)
        {
            var primitiveRosType = RosType.Parse(rosType);
            
            primitiveRosType.IsBuiltIn.Should().BeTrue();
            primitiveRosType.IsArray.Should().BeFalse();
            
            using (var s = new MemoryStream())
            {
                var writer = new RosBinaryWriter(s);
                var reader = new RosBinaryReader(s);
                
                writer.WriteBuiltInType(primitiveRosType, writeValue);

                s.Position = 0;

                var readValue = reader.ReadBuiltInType(primitiveRosType, expectedValue.GetType());

                readValue.Should().BeEquivalentTo(expectedValue);
                readValue.GetType().Should().Be(expectedValue.GetType());
            }
        }

        [Fact]
        public void WriteNullStringGetsEmptyString()
        {
            using (var s = new MemoryStream())
            {
                var writer = new RosBinaryWriter(s);
                var reader = new RosBinaryReader(s);

                writer.WriteBuiltInType(typeof(string), null);

                s.Position = 0;

                var readValue = (string) reader.ReadBuiltInType(typeof(string));

                readValue.Should().BeEmpty();
            }
        }
    }
}