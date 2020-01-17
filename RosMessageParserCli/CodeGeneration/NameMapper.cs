using System;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.TemplateEngines;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class NameMapper : INugetPackageNameResolver, IPackageNamingConvention, ITypeNameResolver
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

        public virtual string ResolveNugetPackageName(string rosPackageName, string rosTypeName)
        {
            return ResolveNugetPackageName(rosPackageName);
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
        
        public string ResolveConcreteTypeName(IRosTypeInfo type)
        {
            return ResolveTypeName(type, false);
        }
        
        public string ResolveInterfacedTypeName(IRosTypeInfo type)
        {
            return ResolveTypeName(type, true);
        }
        
        protected virtual string ResolveTypeName(IRosTypeInfo type, bool useInterface)
        {
            if (type.IsArray)
            {
                return ResolveTypeName(type as IRosArrayTypeInfo, useInterface);
            }
            
            if (type is PrimitiveTypeInfo primitiveType)
            {
                return primitiveType.Type.ToString();
            }
            else if (type is RosTypeInfo rosType)
            {
                var rosPackageName = rosType.PackageName ?? _packageName;
                var rosTypeName = rosType.TypeName;

                return ResolveTypeName(rosPackageName, rosTypeName);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        
        protected virtual string ResolveTypeName(IRosArrayTypeInfo type, bool useInterface)
        {
            if (useInterface)
            {
                return $"System.Collections.Generic.IList<{ResolveTypeName(type.GetUnderlyingType(), true)}>";
            }
            else
            {
                return $"System.Collections.Generic.List<{ResolveTypeName(type.GetUnderlyingType(), false)}>";
            }
        }

        protected virtual string ResolveTypeName(string rosPackageName, string rosTypeName)
        {
            if (rosPackageName == null) throw new ArgumentNullException(nameof(rosPackageName));
            if (rosTypeName == null) throw new ArgumentNullException(nameof(rosTypeName));
             
            var packageName = FormatPackageName(rosPackageName);
            return $"{packageName}.{rosTypeName.ToPascalCase()}";
        }
    }
}