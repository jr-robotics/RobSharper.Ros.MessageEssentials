using System;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public static class RosTypeInfoExtensions
    {
        public static bool SupportsEqualityComparer(this RosTypeInfo type)
        {
            if (type.IsArray || !type.IsBuiltInType)
                return false;

            try
            {
                var typeMapper = BuiltInTypeMapping.Create(type);

                return typeMapper.Type == typeof(string) ||
                       (typeMapper.Type != typeof(double) && typeMapper.Type != typeof(float));
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }

        public static bool IsValueType(this RosTypeInfo type)
        {
            if (type.IsArray || !type.IsBuiltInType)
                return false;

            try
            {
                var typeMapper = BuiltInTypeMapping.Create(type);

                return typeMapper.Type.IsValueType;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }
    }
}