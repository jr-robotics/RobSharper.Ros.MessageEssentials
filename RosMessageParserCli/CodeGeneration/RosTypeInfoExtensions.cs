using System;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public static class RosTypeInfoExtensions
    {
        public static bool SupportsEqualityComparer(this IRosTypeInfo type)
        {
            if (type.IsArray)
                return false;

            if (type.IsPrimitive && type is PrimitiveTypeInfo primitiveType)
            {
                return (primitiveType.Type.IsPrimitive && primitiveType.Type != typeof(double) && primitiveType.Type != typeof(float) )||
                    primitiveType.Type == typeof(string);
            }

            return false;
        }

        public static bool IsValueType(this IRosTypeInfo type)
        {
            if (type.IsArray)
                return false;

            if (type.IsPrimitive && type is PrimitiveTypeInfo primitiveType)
            {
                return primitiveType.Type.IsValueType;
            }

            return false;
        }
    }
}