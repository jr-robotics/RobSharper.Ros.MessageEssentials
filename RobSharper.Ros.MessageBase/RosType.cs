using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using Joanneum.Robotics.Ros.MessageBase.RosTypeParser;

namespace RobSharper.Ros.MessageBase
{
    public class RosType : IFormattable
    {
        private RosType(string packageName, string typeName, bool isBuiltIn, bool isArray, int arraySize)
        {
            if (string.Empty == packageName)
                packageName = null;
            
            PackageName = packageName;
            TypeName = typeName;
            IsBuiltIn = isBuiltIn;
            IsArray = isArray;
            ArraySize = arraySize;
        }

        public string PackageName { get; }
        public string TypeName { get; }
        
        public bool IsBuiltIn { get; }
        public bool IsArray { get; }
        public int ArraySize { get; }

        public bool IsDynamicArray => IsArray && ArraySize == 0;
        public bool IsFixedSizeArray => IsArray && ArraySize > 0;

        public bool IsHeaderType => !IsArray && !IsBuiltIn && PackageName == "std_msgs" && TypeName == "Header";
        
        /// <summary>
        /// Fully qualified ROS type names have package name and type name set or are built in types without a package
        /// </summary>
        public bool IsFullQualified => IsBuiltIn || PackageName != null;

        public RosType ToFullQualifiedType(string packageName)
        {
            if (IsFullQualified)
                throw new InvalidOperationException("MakePackagedType is only valid for non packaged and not built in types.");

            return new RosType(packageName, TypeName, IsBuiltIn, IsArray, ArraySize);
        }

        public override string ToString()
        {
            return ToString("F");
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }
        
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "F";

            format = format.ToUpperInvariant();
            bool omitArraySpecifier;

            switch (format)
            {
                case "F": //Full
                    omitArraySpecifier = false;
                    break;
                case "T": // Type only
                    omitArraySpecifier = true;
                    break;
                default:
                    throw new NotSupportedException($"Format {format} is not supported.");
            }
            
            return ToString(omitArraySpecifier);
        }

        private string ToString(bool omitArraySpecifier)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(PackageName))
            {
                sb.Append(PackageName);
                sb.Append("/");
            }

            sb.Append(TypeName);

            if (!omitArraySpecifier && IsArray)
            {
                sb.Append("[");

                if (ArraySize > 0)
                {
                    sb.Append(ArraySize);
                }

                sb.Append("]");
            }

            return sb.ToString();
        }

        protected bool Equals(RosType other)
        {
            return PackageName == other.PackageName && TypeName == other.TypeName && IsBuiltIn == other.IsBuiltIn && IsArray == other.IsArray && ArraySize == other.ArraySize;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RosType) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (PackageName != null ? PackageName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (TypeName != null ? TypeName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsBuiltIn.GetHashCode();
                hashCode = (hashCode * 397) ^ IsArray.GetHashCode();
                hashCode = (hashCode * 397) ^ ArraySize;
                return hashCode;
            }
        }


        private static readonly IDictionary<string, RosType> RosTypeCache = new Dictionary<string, RosType>();
        
        public static RosType Parse(string type, bool useCache = true)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            type = type.Trim();

            if (useCache && RosTypeCache.TryGetValue(type, out var cachedType))
            {
                return cachedType;
            }

            var input = new AntlrInputStream(type);
            var lexer = new RosTypeLexer(input);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new Joanneum.Robotics.Ros.MessageBase.RosTypeParser.RosTypeParser(tokenStream);
            
            var rosTypeListener = new RosTypeListener();
            parser.AddParseListener(rosTypeListener);

            parser.type_input();

            var rosType = rosTypeListener.GetRosType();

            if (useCache)
            {
                rosType = GetOrSetCacheItem(type, rosType);
            }
            
            return rosType;
        }

        private static RosType GetOrSetCacheItem(string type, RosType rosType)
        {
            lock (RosTypeCache)
            {
                if (RosTypeCache.TryGetValue(type, out var cachedType))
                {
                    return cachedType;
                }
                else
                {
                    RosTypeCache.Add(type, rosType);
                    return rosType;
                }
            }
        }

        private class RosTypeListener : RosTypeBaseListener
        {
            public bool IsBuiltInType { get; private set; }
            public string Package { get; private set; }
            public string Type { get; private set; }
            public bool IsArray { get; private set; }
            public int ArraySize { get; private set; }

            public RosType GetRosType()
            {
                var package = Package;

                if (package == null && "Header".Equals(Type, StringComparison.InvariantCulture))
                    package = "std_msgs";
                
                return new RosType(package, Type, IsBuiltInType, IsArray, ArraySize);
            }


            public override void ExitBuilt_in_type(Joanneum.Robotics.Ros.MessageBase.RosTypeParser.RosTypeParser.Built_in_typeContext context)
            {
                IsBuiltInType = true;
                Type = context.GetText();
            }

            public override void ExitRos_package_type(Joanneum.Robotics.Ros.MessageBase.RosTypeParser.RosTypeParser.Ros_package_typeContext context)
            {
                Package = context.GetChild(0).GetText();
                Type = context.GetChild(2).GetText();
            }

            public override void ExitRos_type(Joanneum.Robotics.Ros.MessageBase.RosTypeParser.RosTypeParser.Ros_typeContext context)
            {
                Package = null;
                Type = context.GetChild(0).GetText();
            }

            public override void ExitVariable_array_type(Joanneum.Robotics.Ros.MessageBase.RosTypeParser.RosTypeParser.Variable_array_typeContext context)
            {
                IsArray = true;
                ArraySize = 0;
            }

            public override void ExitFixed_array_type(RosTypeParser.Fixed_array_typeContext context)
            {
                IsArray = true;
                ArraySize = int.Parse(context.GetChild(2).GetText());
            }
        }
    }
}