using System;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class TypeNameMapper
    {
        private readonly IRosPackageNameResolver _packageNameResolver;

        public TypeNameMapper(IRosPackageNameResolver packageNameResolver)
        {
            _packageNameResolver = packageNameResolver ?? throw new ArgumentNullException(nameof(packageNameResolver));
        }

        public string GetTypeName(IRosArrayTypeInfo type)
        {
            return $"System.Collections.Generic.IList<{GetTypeName(type.GetUnderlyingType())}>";
        }
        
        public string GetTypeName(IRosTypeInfo type)
        {
            if (type.IsArray)
            {
                return GetTypeName(type as IRosArrayTypeInfo);
            }
            
            if (type is PrimitiveTypeInfo primitiveType)
            {
                return primitiveType.Type.ToString();
            }
            else if (type is RosTypeInfo rosType)
            {
                return $"{_packageNameResolver.ResolvePackageName(rosType.PackageName)}.{rosType.TypeName}";
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}