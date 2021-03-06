using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RobSharper.Ros.MessageEssentials.Serialization
{
    public class RosBinaryWriter : BinaryWriter
    {
        private readonly LittleEndianStream _stream;
        private bool _writesString = false;

        public RosBinaryWriter(Stream stream) : base(new LittleEndianStream(stream), Encoding.ASCII, true)
        {
            _stream = (LittleEndianStream) base.BaseStream;
        }

        public override void Write(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            
            Write(value.Length);

            using (_stream.UseMode(LittleEndianStream.WriteModes.DoNotConvert))
            {
                try
                {
                    _writesString = true;
                    base.Write(value);
                }
                finally
                {
                    _writesString = false;
                }
            }
        }

        public override void Write(byte value)
        {
            // base.WriteString writes the length in a variable bit size
            // we do not want this to happen.
            if (_writesString)
                return;

            base.Write(value);
        }

        public override void Write(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            
            using (_stream.DoNotConvert())
            {
                base.Write(buffer);
            }
        }

        public override void Write(byte[] buffer, int index, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            
            using (_stream.DoNotConvert())
            {
                base.Write(buffer, index, count);
            }
        }

        public override void Write(char[] chars)
        {
            if (chars == null)
                throw new ArgumentNullException(nameof(chars));
            
            CheckChars(chars, 0, chars.Length);

            using (_stream.DoNotConvert())
            {
                base.Write(chars);
            }
        }

        public override void Write(char[] chars, int index, int count)
        {
            if (chars == null)
                throw new ArgumentNullException(nameof(chars));
            
            CheckChars(chars, index, count);

            using (_stream.DoNotConvert())
            {
                base.Write(chars, index, count);
            }
        }

        public override void Write(char ch)
        {
            CheckChars(new[] {ch}, 0, 1);

            base.Write(ch);
        }

        public override void Write(decimal value)
        {
            throw new NotSupportedException("Decimals are not supported by ROS");
        }

        /// <summary>
        /// Writes a DateTime as ROS time value.
        /// Uses <see cref="RosTimeExtensions.ToRosTime"/> for conversion to a ROS time. 
        /// </summary>
        /// <param name="time"></param>
        /// <exception cref="NotSupportedException"></exception>
        public void Write(DateTime time)
        {
            var rosTime = time.ToRosTime();
            Write(rosTime);
        }

        public void Write(RosTime time)
        {
            Write(time.Seconds);
            Write(time.Nanoseconds);
        }
        
        public void Write(TimeSpan timeSpan)
        {
            var rosDuration = timeSpan.ToRosDuration();
            Write(rosDuration);
        }

        public void Write(RosDuration duration)
        {
            Write(duration.Seconds);
            Write(duration.Nanoseconds);
        }

        public void WriteBuiltInType(Type type, object value)
        {
            var writeAction = Serializers[type];
            writeAction(this, value);
        }

        public void WriteBuiltInType(RosType rosType, object value)
        {
            var type = BuiltInRosTypes.GetSerializationType(rosType);
            
            if (value != null)
            {
                var valueType = value.GetType();
                
                if (valueType != type)
                {
                    value = Convert.ChangeType(value, type);
                }
            }

            WriteBuiltInType(type, value);
        }

        /// <summary>
        /// All characters must be 1 byte ascii chars
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <exception cref="NotSupportedException"></exception>
        private void CheckChars(char[] chars, int index, int count)
        {
            if (Encoding.ASCII.GetByteCount(chars, index, count) > count - index)
                throw new NotSupportedException("Only ASCII characters are allowed");
        }

        private static readonly Dictionary<Type, Action<RosBinaryWriter, object>> Serializers =
            new Dictionary<Type, Action<RosBinaryWriter, object>>
            {
                {
                    typeof(bool),
                    (writer, value) => writer.Write((bool) value)
                },

                {
                    typeof(short),
                    (writer, value) => writer.Write((short) value)
                },
                {
                    typeof(sbyte),
                    (writer, value) => writer.Write((sbyte) value)
                },
                {
                    typeof(int),
                    (writer, value) => writer.Write((int) value)
                },
                {
                    typeof(long),
                    (writer, value) => writer.Write((long) value)
                },
                {
                    typeof(byte),
                    (writer, value) => writer.Write((byte) value)
                },
                {
                    typeof(ushort),
                    (writer, value) => writer.Write((ushort) value)
                },
                {
                    typeof(uint),
                    (writer, value) => writer.Write((uint) value)
                },
                {
                    typeof(ulong),
                    (writer, value) => writer.Write((ulong) value)
                },
                {
                    typeof(float),
                    (writer, value) => writer.Write((float) value)
                },
                {
                    typeof(double),
                    (writer, value) => writer.Write((double) value)
                },
                {
                    typeof(string),
                    (writer, value) => writer.Write((string) value)
                },
                {
                    typeof(DateTime),
                    (writer, value) => writer.Write((DateTime) value)
                },
                {
                    typeof(RosTime),
                    (writer, value) => writer.Write((RosTime) value)
                },
                {
                    typeof(TimeSpan),
                    (writer, value) => writer.Write((TimeSpan) value)
                },
                {
                    typeof(RosDuration),
                    (writer, value) => writer.Write((RosDuration) value)
                }
            };
    }
}