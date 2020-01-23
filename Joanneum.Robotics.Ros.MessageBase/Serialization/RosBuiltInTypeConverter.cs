using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Joanneum.Robotics.Ros.MessageBase.Serialization
{
    public static class RosBuiltInTypeConverter
    {
        private struct Serializer
        {
            public Action<Stream, object> Serialize { get; set; }
            public Func<Stream, object> Deserialize { get; set; }
        }
        
        private static readonly Dictionary<Type, Serializer> Serializers = new Dictionary<Type, Serializer>()
        {
            {
                typeof(bool),
                new Serializer
                {
                    Serialize = (s, o) => WriteBytes(s, (bool)o),
                    Deserialize = s => ReadBool(s)
                }
            },

            {
                typeof(short),
                new Serializer
                {
                    Serialize = (s, o) => WriteBytes(s, (short)o),
                    Deserialize = s => ReadInt8(s)
                }
            },
            {
                typeof(sbyte),
                new Serializer
                {
                    Serialize = (s, o) => WriteBytes(s, (sbyte)o) ,
                    Deserialize = s => ReadInt16(s)
                }
            },
            {
                typeof(int),
                new Serializer
                {
                    Serialize = (s, o) => WriteBytes(s, (int)o),
                    Deserialize = s => ReadInt32(s)
                }
            },
            {
                typeof(long),
                new Serializer
                {
                    Serialize = (s, o) => WriteBytes(s, (long)o),
                    Deserialize = s => ReadInt64(s)
                }
            },
            {
                typeof(byte),
                new Serializer
                {
                    Serialize = (s, o) => WriteBytes(s, (byte)o),
                    Deserialize = s => ReadUInt8(s)
                }
            },
            {
                typeof(ushort),
                new Serializer
                {
                    Serialize = (s, o) => WriteBytes(s, (ushort)o),
                    Deserialize = s => ReadUInt16(s)
                }
            },
            {
                typeof(uint),
                new Serializer
                {
                    Serialize = (s, o) => WriteBytes(s, (uint)o),
                    Deserialize = s => ReadUInt32(s)
                }
            },
            {
                typeof(ulong),
                new Serializer
                {
                    Serialize = (s, o) => WriteBytes(s, (ulong)o),
                    Deserialize = s => ReadUInt64(s)
                }
            },
            {
                typeof(float),
                new Serializer
                {
                    Serialize = (s, o) => WriteBytes(s, (float)o),
                    Deserialize = s => ReadSingle(s)
                }
            },
            {
                typeof(double),
                new Serializer
                {
                    Serialize = (s, o) => WriteBytes(s, (double)o),
                    Deserialize = s => ReadDouble(s)
                }
            },
            {
                typeof(string),
                new Serializer
                {
                    Serialize = (s, o) => WriteBytes(s, (string) o),
                    Deserialize = s => ReadString(s)
                }
            }
        };

        public static void WriteBytes(Stream s, Type t, object o)
        {
            var formatter = Serializers[t];
            formatter.Serialize(s, o);
        }

        public static object ReadValue(Stream input, Type type)
        {
            var serializer = Serializers[type];
            return serializer.Deserialize(input);
        }

        #region BOOL

        public static byte[] GetBytes(bool value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }

        public static bool ToBool(byte[] buffer, int startIndex = 0)
        {
            buffer.FromLittleEndian();
            return BitConverter.ToBoolean(buffer, startIndex);
        }

        public static void WriteBytes(Stream destination, bool value)
        {
            destination.Write(GetBytes(value));
        }

        public static bool ReadBool(Stream stream)
        {
            var b = stream.ReadBytes(1);
            return ToBool(b);
        }

        #endregion

        
        #region Int8

        public static byte[] GetBytes(sbyte value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }

        public static sbyte ToInt8(byte[] buffer)
        {
            buffer.FromLittleEndian();
            return (sbyte) buffer[0];
        }
        
        public static void WriteBytes(Stream destination, sbyte value)
        {
            destination.Write(GetBytes(value));
        }

        public static sbyte ReadInt8(Stream stream)
        {
            var b = stream.ReadBytes(1);
            return ToInt8(b);
        }

        #endregion


        #region Int16

        public static byte[] GetBytes(short value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }

        public static short ToInt16(byte[] buffer)
        {
            buffer.FromLittleEndian();
            return BitConverter.ToInt16(buffer, 0);
        }

        public static void WriteBytes(Stream destination, short value)
        {
            destination.Write(GetBytes(value));
        }

        public static short ReadInt16(Stream stream)
        {
            var b = stream.ReadBytes(2);
            return ToInt16(b);
        }

        #endregion


        #region Int32

        public static byte[] GetBytes(int value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }

        public static int ToInt32(byte[] buffer)
        {
            buffer.FromLittleEndian();
            return BitConverter.ToInt32(buffer, 0);
        }
        
        public static void WriteBytes(Stream destination, int value)
        {
            destination.Write(GetBytes(value));
        }

        public static int ReadInt32(Stream stream)
        {
            var b = stream.ReadBytes(4);
            return ToInt32(b);
        }

        #endregion


        #region Int64

        public static byte[] GetBytes(long value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }
        
        public static long ToInt64(byte[] buffer)
        {
            buffer.FromLittleEndian();
            return BitConverter.ToInt64(buffer, 0);
        }

        public static void WriteBytes(Stream destination, long value)
        {
            destination.Write(GetBytes(value));
        }

        public static long ReadInt64(Stream stream)
        {
            var b = stream.ReadBytes(8);
            return ToInt64(b);
        }
        
        #endregion


        #region UInt8

        public static byte[] GetBytes(byte value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }
        
        public static byte ToUInt8(byte[] buffer)
        {
            buffer.FromLittleEndian();
            return buffer[0];
        }
        
        public static void WriteBytes(Stream destination, byte value)
        {
            destination.Write(GetBytes(value));
        }

        public static byte ReadUInt8(Stream stream)
        {
            var b = stream.ReadBytes(1);
            return ToUInt8(b);
        }

        #endregion


        #region UInt16

        public static byte[] GetBytes(ushort value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }
        
        public static ushort ToUInt16(byte[] buffer)
        {
            buffer.FromLittleEndian();
            return BitConverter.ToUInt16(buffer, 0);
        }
        
        public static void WriteBytes(Stream destination, ushort value)
        {
            destination.Write(GetBytes(value));
        }

        public static ushort ReadUInt16(Stream stream)
        {
            var b = stream.ReadBytes(2);
            return ToUInt16(b);
        }

        #endregion


        #region UInt32

        public static byte[] GetBytes(uint value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }
        
        public static uint ToUInt32(byte[] buffer)
        {
            buffer.FromLittleEndian();
            return BitConverter.ToUInt32(buffer, 0);
        }
        
        public static void WriteBytes(Stream destination, uint value)
        {
            destination.Write(GetBytes(value));
        }

        public static uint ReadUInt32(Stream stream)
        {
            var b = stream.ReadBytes(4);
            return ToUInt32(b);
        }

        #endregion


        #region UInt64

        public static byte[] GetBytes(ulong value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }
        
        public static ulong ToUInt64(byte[] buffer)
        {
            buffer.FromLittleEndian();
            return BitConverter.ToUInt64(buffer, 0);
        }
        
        public static void WriteBytes(Stream destination, ulong value)
        {
            destination.Write(GetBytes(value));
        }

        public static ulong ReadUInt64(Stream stream)
        {
            var b = stream.ReadBytes(8);
            return ToUInt64(b);
        }

        #endregion


        #region Float

        public static byte[] GetBytes(float value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }
        
        public static float ToSingle(byte[] buffer)
        {
            buffer.FromLittleEndian();
            return BitConverter.ToSingle(buffer, 0);
        }
        
        public static void WriteBytes(Stream destination, float value)
        {
            destination.Write(GetBytes(value));
        }

        public static float ReadSingle(Stream stream)
        {
            var b = stream.ReadBytes(4);
            return ToSingle(b);
        }

        #endregion


        #region Double

        public static byte[] GetBytes(double value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }
        
        public static double ToDouble(byte[] buffer)
        {
            buffer.FromLittleEndian();
            return BitConverter.ToDouble(buffer, 0);
        }
        
        public static void WriteBytes(Stream destination, double value)
        {
            destination.Write(GetBytes(value));
        }

        public static double ReadDouble(Stream stream)
        {
            var b = stream.ReadBytes(4);
            return ToDouble(b);
        }

        #endregion


        #region String

        public static void WriteBytes(Stream destination, string value)
        {
            if (value == null)
                value = string.Empty;

            WriteBytes(destination, value.Length);
            
            var valueBytes = Encoding.ASCII
                .GetBytes(value)
                .ToLittleEndian();

            destination.Write(valueBytes);
        }

        public static string ReadString(Stream stream)
        {
            var length = ReadInt32(stream);

            if (length == 0)
                return string.Empty;

            var buffer = stream.ReadBytes(length);
            return Encoding.ASCII.GetString(buffer);
        }

        #endregion

    }
}