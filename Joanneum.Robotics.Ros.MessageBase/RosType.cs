using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using Joanneum.Robotics.Ros.MessageBase.RosTypeParser;
using Microsoft.Extensions.Primitives;

namespace Joanneum.Robotics.Ros.MessageBase
{
    public class RosType
    {
        private class RosTypeListener : RosTypeBaseListener
        {
            public bool IsBuiltInType { get; private set; }
            public string Package { get; private set; }
            public string Type { get; private set; }
            public bool IsArray { get; private set; }
            public int ArraySize { get; private set; }


            public override void ExitBuilt_in_type(RosTypeParser.RosTypeParser.Built_in_typeContext context)
            {
                IsBuiltInType = true;
                Type = context.GetText();
            }

            public override void ExitRos_package_type(RosTypeParser.RosTypeParser.Ros_package_typeContext context)
            {
                Package = context.GetChild(0).GetText();
                Type = context.GetChild(2).GetText();
            }

            public override void ExitRos_type(RosTypeParser.RosTypeParser.Ros_typeContext context)
            {
                Package = null;
                Type = context.GetChild(0).GetText();
            }

            public override void ExitVariable_array_type(RosTypeParser.RosTypeParser.Variable_array_typeContext context)
            {
                IsArray = true;
                ArraySize = 0;
            }
        }
        
        private RosType(string packageName, string typeName, bool isBuiltIn, bool isArray, int arraySize, Type mappedType)
        {
            PackageName = packageName;
            TypeName = typeName;
            IsBuiltIn = isBuiltIn;
            IsArray = isArray;
            ArraySize = arraySize;
            MappedType = mappedType;
        }

        public string PackageName { get; }
        public string TypeName { get; }
        
        public bool IsBuiltIn { get; }
        public bool IsArray { get; }
        public int ArraySize { get; }
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

                if (IsArray)
                {
                    sb.Append("[");
                    
                    if (ArraySize > 0)
                    {
                        sb.Append(ArraySize);
                    }
                    
                    sb.Append("]");
                }
                _stringValue = sb.ToString();
            }
            
            return _stringValue;
        }

        public static RosType Create(string type, Type mappedType)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var input = new AntlrInputStream(type);
            var lexer = new RosTypeLexer(input);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new RosTypeParser.RosTypeParser(tokenStream);
            
            var rosTypeListener = new RosTypeListener();
            parser.AddParseListener(rosTypeListener);

            parser.type_input();
            
            return Create(rosTypeListener.Package, rosTypeListener.Type, rosTypeListener.IsArray, rosTypeListener.ArraySize, mappedType);
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

            return new RosType(package, type, isBuiltIn, isArray, arraySize, mappedType);
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