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
    }
}