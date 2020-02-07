using System;
using RobSharper.Ros.MessageCli.CodeGeneration.TemplateEngines;
using RobSharper.Ros.MessageParser;

namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    public class NameMapper : INugetPackageNameResolver, IResourceNamingConvention, ITypeNameResolver, IBuiltInTypeChecker, IRosNamingConvention
    {
        private readonly string _packageName;
        private readonly ITemplateFormatter _packageNamingConvention;

        public NameMapper(string packageName, ITemplateFormatter packageNamingConvention)
        {
            _packageName = packageName ?? throw new ArgumentNullException(nameof(packageName));
            _packageNamingConvention = packageNamingConvention ?? throw new ArgumentNullException(nameof(packageNamingConvention));
        }
         
        public virtual string ResolveNugetPackageName(string rosPackageName)
        {
            if (rosPackageName == null) throw new ArgumentNullException(nameof(rosPackageName));

            return GetNamespace(rosPackageName);
        }

        public virtual string ResolveNugetPackageName(RosTypeInfo rosType)
        {
            return ResolveNugetPackageName(rosType.PackageName);
        }

        public virtual string GetNamespace(string rosPackageName)
        {
            var data = new
            {
                Name = rosPackageName,
                PascalName = rosPackageName.ToPascalCase()
            };

            return _packageNamingConvention
                .Format(data)
                .Trim();
        }

        public string GetTypeName(string rosTypeName)
        {
            return GetTypeName(rosTypeName, DetailedRosMessageType.None);
        }

        public virtual string GetTypeName(string rosTypeName, DetailedRosMessageType messageType)
        {
            if (rosTypeName == null) throw new ArgumentNullException(nameof(rosTypeName));
            
            var typeName = rosTypeName.ToPascalCase();

            switch (messageType)
            {
                case DetailedRosMessageType.ActionGoal:
                    typeName += "Goal";
                    break;
                case DetailedRosMessageType.ActionResult:
                    typeName += "Result";
                    break;
                case DetailedRosMessageType.ActionFeedback:
                    typeName += "Feedback";
                    break;
                case DetailedRosMessageType.ServiceRequest:
                    typeName += "Request";
                    break;
                case DetailedRosMessageType.ServiceResponse:
                    typeName += "Response";
                    break;
            }
            
            return typeName;
        }

        public string ResolveFullQualifiedTypeName(RosTypeInfo type)
        {
            return ResolveFullQualifiedName(type, false);
        }
        
        public string ResolveFullQualifiedInterfaceName(RosTypeInfo type)
        {
            return ResolveFullQualifiedName(type, true);
        }
        
        protected virtual string ResolveFullQualifiedName(RosTypeInfo type, bool useInterface)
        {
            string typeString;

            if (type.IsBuiltInType)
            {
                var typeMapper = BuiltInTypeMapping.Create(type);
                typeString = typeMapper.Type.ToString();
            }
            else
            {
                var rosPackageName = type.PackageName ?? _packageName;
                var rosTypeName = type.TypeName;

                typeString = ResolveFullQualifiedName(rosPackageName, rosTypeName);
            }

            if (type.IsArray)
            {
                if (useInterface)
                {
                    typeString = $"System.Collections.Generic.IList<{typeString}>";
                }
                else
                {
                    typeString = $"System.Collections.Generic.List<{typeString}>";
                }
            }

            return typeString;
        }

        protected virtual string ResolveFullQualifiedName(string rosPackageName, string rosTypeName)
        {
            if (rosPackageName == null) throw new ArgumentNullException(nameof(rosPackageName));
            if (rosTypeName == null) throw new ArgumentNullException(nameof(rosTypeName));
             
            var namespaceName = GetNamespace(rosPackageName);
            var typeName = GetTypeName(rosTypeName);
            
            return $"{namespaceName}.{typeName}";
        }

        public virtual bool IsBuiltInType(RosTypeInfo rosType)
        {
            if (rosType == null) throw new ArgumentNullException(nameof(rosType));

            return rosType.IsBuiltInType;
        }

        public virtual string GetRosTypeName(string rosTypeName, DetailedRosMessageType messageType)
        {
            switch (messageType)
            {
                case DetailedRosMessageType.None:
                case DetailedRosMessageType.Message:
                    return rosTypeName;
                case DetailedRosMessageType.ActionGoal:
                    return rosTypeName + "Goal";
                case DetailedRosMessageType.ActionResult:
                    return rosTypeName + "Result";
                case DetailedRosMessageType.ActionFeedback:
                    return rosTypeName + "Feedback";
                case DetailedRosMessageType.ServiceRequest:
                    return rosTypeName + "Request";
                case DetailedRosMessageType.ServiceResponse:
                    return rosTypeName + "Response";
                default:
                    throw new NotSupportedException($"Message type {messageType} is not supported.");
            }
        }
    }
}