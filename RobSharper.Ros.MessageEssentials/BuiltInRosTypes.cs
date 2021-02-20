using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RobSharper.Ros.MessageEssentials
{
    public static class BuiltInRosTypes
    {
        public static readonly RosType Bool = RosType.Parse("bool");
        public static readonly RosType Int8 = RosType.Parse("int8");
        public static readonly RosType UInt8 = RosType.Parse("uint8");
        public static readonly RosType Int16 = RosType.Parse("int16");
        public static readonly RosType UInt16 = RosType.Parse("uint16");
        public static readonly RosType Int32 = RosType.Parse("int32");
        public static readonly RosType UInt32 = RosType.Parse("uint32");
        public static readonly RosType Int64 = RosType.Parse("int64");
        public static readonly RosType UInt64 = RosType.Parse("uint64");
        public static readonly RosType Float32 = RosType.Parse("float32");
        public static readonly RosType Float64 = RosType.Parse("float64");
        public static readonly RosType String = RosType.Parse("string");
        public static readonly RosType Time = RosType.Parse("time");
        public static readonly RosType Duration = RosType.Parse("duration");
        
        // Deprecated ROS Types
        public static readonly RosType Byte = RosType.Parse("byte");
        public static readonly RosType Char = RosType.Parse("char");

        public static readonly Dictionary<string, Type> RosTypeMappings = new Dictionary<string, Type>
        {
            {"bool", typeof(bool)},
            {"int8", typeof(sbyte)},
            {"uint8", typeof(byte)},
            {"int16", typeof(short)},
            {"uint16", typeof(ushort)},
            {"int32", typeof(int)},
            {"uint32", typeof(uint)},
            {"int64", typeof(long)},
            {"uint64", typeof(ulong)},
            {"float32", typeof(float)},
            {"float64", typeof(double)},
            {"string", typeof(string)},
            {"time", typeof(DateTime)},
            {"duration", typeof(TimeSpan)},
            
            {"byte", typeof(sbyte)},
            {"char", typeof(byte)}
        };
        
        public static Type GetSerializationType(RosType rosType)
        {
            if (RosTypeMappings.TryGetValue(rosType.TypeName, out var type))
            {
                Debug.Assert(type != null, nameof(type) + " != null");
                return type;
            }
            
            // Type was not found. Maybe it was not a built in ros type
            if (!rosType.IsBuiltIn)
                throw new InvalidOperationException($"ROS type {rosType} is not a built in ROS type.");

            throw new InvalidOperationException($"ROS type {rosType} is not supported.");
        }
    }
}