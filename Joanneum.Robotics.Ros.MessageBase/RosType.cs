using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Primitives;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosType
    {
        private RosType(string packageName, string typeName, bool isBuiltIn, bool isArray, Type mappedType)
        {
            PackageName = packageName;
            TypeName = typeName;
            IsBuiltIn = isBuiltIn;
            IsArray = isArray;
            MappedType = mappedType;
        }

        public string PackageName { get; }
        public string TypeName { get; }
        
        public bool IsBuiltIn { get; }
        public bool IsArray { get; }
        public Type MappedType { get; }

        private string _stringValue;

        public override string ToString()
        {
            if (_stringValue == null)
            {
                var sb = new StringBuilder();

                if (!string.IsNullOrEmpty(PackageName))
                {
                    sb.Append(PackageName);
                    sb.Append(" ");
                }

                sb.Append(TypeName);
                
                _stringValue = sb.ToString();
            }
            
            return _stringValue;
        }


        [Obsolete]
        public static RosType Create(string type, Type mappedType)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            
            var typeParts = type.Split('/');

            string rosPackage;
            string rosType;
            bool isArray;
            int arraySize;
            
            switch (typeParts.Length)
            {
                case 1:
                    rosPackage = null;
                    rosType = typeParts[0];
                    break;
                case 2:
                    rosPackage = typeParts[0];
                    rosType = typeParts[1];
                    break;
                default:
                    throw new InvalidOperationException($"Could not parse ros type '{type}'");
            }
            
            
            return Create(rosPackage, rosType, mappedType);
        }

        public static RosType Create(string package, string type, Type mappedType)
        {
            return Create(package, type, false, 0, mappedType);
        }

        public static RosType Create(string package, string type, bool isArray, int arraySize, Type mappedType)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (mappedType == null) throw new ArgumentNullException(nameof(mappedType));
            
            if (!isArray && arraySize > 0)
                throw new InvalidOperationException("isArray must be true if array size > 0");

            if (package == string.Empty)
                package = null;
            
            var isBuiltIn = false;

            // TODO: Handle arrays here or somewhere else?
            
            if (package == null)
            {
                isBuiltIn = IsBuiltInType(type);
            }

            // TODO allow array size
            return new RosType(package, type, isBuiltIn, isArray, mappedType);
        }

        private static readonly HashSet<string> _builtInTypes = new HashSet<string>()
        {
            "int8",
            "int16",
            "int32",
            "int64",
            "uint8",
            "uint16",
            "uint32",
            "uint64",
            "float32",
            "float64",
            "string",
            "time",
            "duration",
            "bool",
            
            "char",
            "byte"
        };
        
        public static bool IsBuiltInType(string type)
        {
            return _builtInTypes.Contains(type);
        }
    }
}