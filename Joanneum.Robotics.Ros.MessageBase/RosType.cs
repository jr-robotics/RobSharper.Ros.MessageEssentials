using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Primitives;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosType
    {
        private RosType(string packageName, string typeName, bool isBuiltIn, Type mappedType)
        {
            PackageName = packageName;
            TypeName = typeName;
            IsBuiltIn = isBuiltIn;
            MappedType = mappedType;
        }

        public string PackageName { get; }
        public string TypeName { get; }
        
        public bool IsBuiltIn { get; }
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


        public static RosType Create(string type, Type mappedType)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            
            var typeParts = type.Split('/');

            switch (typeParts.Length)
            {
                case 1:
                    return Create(null, typeParts[0], mappedType);
                case 2:
                    return Create(typeParts[0], typeParts[1], mappedType);
                default:
                    throw new InvalidOperationException($"Could not parse ros type '{type}'");
            }
        }

        public static RosType Create(string package, string type, Type mappedType)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (mappedType == null) throw new ArgumentNullException(nameof(mappedType));

            if (package == string.Empty)
                package = null;
            
            var isBuiltIn = false;

            // TODO: Handle arrays here or somewhere else?
            
            if (package == null)
            {
                isBuiltIn = IsBuiltInType(type);
            }

            return new RosType(package, type, isBuiltIn, mappedType);
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