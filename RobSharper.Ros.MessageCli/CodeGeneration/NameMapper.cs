using System;
using RobSharper.Ros.MessageCli.CodeGeneration.TemplateEngines;
using RobSharper.Ros.MessageParser;

namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    public class NameMapper : INugetPackageNameResolver, IPackageNamingConvention, ITypeNameResolver, IBuiltInTypeChecker
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

            return FormatPackageName(rosPackageName);
        }

        public virtual string ResolveNugetPackageName(RosTypeInfo rosType)
        {
            return ResolveNugetPackageName(rosType.PackageName);
        }

        public virtual string FormatPackageName(string rosPackageName)
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
        
        public string ResolveConcreteTypeName(RosTypeInfo type)
        {
            return ResolveTypeName(type, false);
        }
        
        public string ResolveInterfacedTypeName(RosTypeInfo type)
        {
            return ResolveTypeName(type, true);
        }
        
        protected virtual string ResolveTypeName(RosTypeInfo type, bool useInterface)
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

                typeString = ResolveTypeName(rosPackageName, rosTypeName);
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

        protected virtual string ResolveTypeName(string rosPackageName, string rosTypeName)
        {
            if (rosPackageName == null) throw new ArgumentNullException(nameof(rosPackageName));
            if (rosTypeName == null) throw new ArgumentNullException(nameof(rosTypeName));
             
            var packageName = FormatPackageName(rosPackageName);
            return $"{packageName}.{rosTypeName.ToPascalCase()}";
        }

        public virtual bool IsBuiltInType(RosTypeInfo rosType)
        {
            if (rosType == null) throw new ArgumentNullException(nameof(rosType));

            return rosType.IsBuiltInType;
        }
    }
}