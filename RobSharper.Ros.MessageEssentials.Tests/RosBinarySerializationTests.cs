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
    }
}