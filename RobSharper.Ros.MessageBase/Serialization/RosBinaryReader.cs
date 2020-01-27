using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RobSharper.Ros.MessageBase.Serialization
{
    public class RosBinaryReader : BinaryReader
    {
        private LittleEndianStream _stream;
        
        private int _stringLengthIndex = -1;
        private byte[] _stringLength;
        

        public RosBinaryReader(Stream input) : base(new LittleEndianStream(input), Encoding.ASCII, true)
        {
            _stream = (LittleEndianStream) base.BaseStream;
        }

        public override byte ReadByte()
        {
            if (_stringLengthIndex >= 0)
            {
                return _stringLength[_stringLengthIndex];
            }
            
            return base.ReadByte();
        }

        public override int Read(byte[] buffer, int index, int count)
        {
            using (_stream.DoNotConvert())
            {
                return base.Read(buffer, index, count);
            }
        }

        public override int Read(char[] buffer, int index, int count)
        {
            using (_stream.DoNotConvert())
            {
                return base.Read(buffer, index, count);
            }
        }

        public override byte[] ReadBytes(int count)
        {
            using (_stream.DoNotConvert())
            {
                return base.ReadBytes(count);
            }
        }

        public override decimal ReadDecimal()
        {
            throw new NotSupportedException("Decimals are not supported by ROS");
        }

        public override string ReadString()
        {
            using (_stream.DoNotConvert())
            {
                try
                {
                    _stringLengthIndex = 0;
                    _stringLength = Get7BitEncodedInt(ReadInt32());
                    
                    return base.ReadString();
                }
                finally
                {
                    _stringLengthIndex = -1;
                }
            }
        }

        private byte[] Get7BitEncodedInt(int value)
        {
            var res = new byte[5];
            var index = 0;
            
            // Write out an int 7 bits at a time.  The high bit of the byte,
            // when on, tells reader to continue reading more bytes.
            uint v = (uint)value;   // support negative numbers
            while (v >= 0x80)
            {
                res[index++] = (byte) (v | 0x80);
                v >>= 7;
            }

            res[index++] = (byte) v;

            return res;
        }
        
        private static readonly Dictionary<Type, Func<RosBinaryReader, object>> Serializers =
            new Dictionary<Type, Func<RosBinaryReader, object>>
            {
                {
                    typeof(bool),
                    reader => reader.ReadBoolean()
                },

                {
                    typeof(short),
                    reader => reader.ReadByte()
                },
                {
                    typeof(sbyte),
                    reader => reader.ReadInt16()
                },
                {
                    typeof(int),
                    reader => reader.ReadInt32()
                },
                {
                    typeof(long),
                    reader => reader.ReadInt64()
                },
                {
                    typeof(byte),
                    reader => reader.ReadByte()
                },
                {
                    typeof(ushort),
                    reader => reader.ReadUInt16()
                },
                {
                    typeof(uint),
                    reader => reader.ReadUInt32()
                },
                {
                    typeof(ulong),
                    reader => reader.ReadUInt64()
                },
                {
                    typeof(float),
                    reader => reader.ReadSingle()
                },
                {
                    typeof(double),
                    reader => reader.ReadDouble()
                },
                {
                    typeof(string),
                    reader => reader.ReadString()
                }
            };

        public object ReadBuiltInType(Type type)
        {
            var readFunction = Serializers[type];
            return readFunction(this);
        }
    }
}