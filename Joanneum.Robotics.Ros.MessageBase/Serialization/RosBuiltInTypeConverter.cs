using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Joanneum.Robotics.Ros.MessageBase.Serialization
{
    public static class RosBuiltInTypeConverter
    {
        private static readonly Dictionary<Type, Action<Stream, object>> PrimitiveTypeConverters = new Dictionary<Type, Action<Stream, object>>()
        {
            {typeof(bool), (s, o) => WriteBytes(s, (bool)o)},
            {typeof(short), (s, o) => WriteBytes(s, (short)o)},
            {typeof(sbyte), (s, o) => WriteBytes(s, (sbyte)o)},
            {typeof(int), (s, o) => WriteBytes(s, (int)o)},
            {typeof(long), (s, o) => WriteBytes(s, (long)o)},
            {typeof(byte), (s, o) => WriteBytes(s, (byte)o)},
            {typeof(ushort), (s, o) => WriteBytes(s, (ushort)o)},
            {typeof(uint), (s, o) => WriteBytes(s, (uint)o)},
            {typeof(ulong), (s, o) => WriteBytes(s, (ulong)o)},
            {typeof(float), (s, o) => WriteBytes(s, (float)o)},
            {typeof(double), (s, o) => WriteBytes(s, (double)o)},
            {typeof(char), (s, o) => WriteBytes(s, (char)o)},
            {typeof(string), (s, o) => WriteBytes(s, (string) o)}
        };
        
        public static void WriteBytes(Stream s, Type t, object o)
        {
            var action = PrimitiveTypeConverters[t];
            action(s, o);
        }

        public static byte[] GetBytes(bool value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }

        public static byte[] GetBytes(short value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }

        public static byte[] GetBytes(sbyte value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }

        public static byte[] GetBytes(int value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }

        public static byte[] GetBytes(long value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }

        public static byte[] GetBytes(byte value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }

        public static byte[] GetBytes(ushort value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }

        public static byte[] GetBytes(uint value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }

        public static byte[] GetBytes(ulong value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }

        public static byte[] GetBytes(float value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }

        public static byte[] GetBytes(double value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }

        public static byte[] GetBytes(char value)
        {
            return BitConverter
                .GetBytes(value)
                .ToLittleEndian();
        }

        public static void WriteBytes(Stream destination, bool value)
        {
            destination.Write(GetBytes(value));
        }

        public static void WriteBytes(Stream destination, short value)
        {
            destination.Write(GetBytes(value));
        }

        public static void WriteBytes(Stream destination, sbyte value)
        {
            destination.Write(GetBytes(value));
        }

        public static void WriteBytes(Stream destination, int value)
        {
            destination.Write(GetBytes(value));
        }

        public static void WriteBytes(Stream destination, long value)
        {
            destination.Write(GetBytes(value));
        }

        public static void WriteBytes(Stream destination, byte value)
        {
            destination.Write(GetBytes(value));
        }

        public static void WriteBytes(Stream destination, ushort value)
        {
            destination.Write(GetBytes(value));
        }

        public static void WriteBytes(Stream destination, uint value)
        {
            destination.Write(GetBytes(value));
        }

        public static void WriteBytes(Stream destination, ulong value)
        {
            destination.Write(GetBytes(value));
        }
        
        public static void WriteBytes(Stream destination, float value)
        {
            destination.Write(GetBytes(value));
        }

        public static void WriteBytes(Stream destination, double value)
        {
            destination.Write(GetBytes(value));
        }

        public static void WriteBytes(Stream destination, char value)
        {
            destination.Write(GetBytes(value));
        }

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
    }
}