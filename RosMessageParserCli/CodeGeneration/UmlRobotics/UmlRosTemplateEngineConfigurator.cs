using System;
using HandlebarsDotNet;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.TemplateEngines;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.UmlRobotics
{
    public class UmlRosTemplateEngineConfigurator
    {
        private readonly IRosPackageNameResolver _packageResolver;

        public UmlRosTemplateEngineConfigurator(IRosPackageNameResolver packageResolver)
        {
            _packageResolver = packageResolver ?? throw new ArgumentNullException(nameof(packageResolver));
        }

        public void Configure(HandlebarsConfiguration configuration)
        {
            configuration.Helpers.Add("mapIdentifier", (output, context, arguments) =>
            {
                var identifier = arguments[0].ToString();
                output.WriteSafeString(identifier.ToPascalCase());
            });
            
            configuration.Helpers.Add("mapType", (output, context, arguments) =>
            {
                var rosType = (IRosTypeInfo) arguments[0];
                output.WriteSafeString(GetTypeName(rosType));
            });
        }

        private string GetTypeName(IRosArrayTypeInfo type)
        {
            return $"System.Collections.Generic.IList<{GetTypeName(type.GetUnderlyingType())}>";
        }
        
        private string GetTypeName(IRosTypeInfo type)
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
                return $"{_packageResolver.ResolvePackageName(rosType.PackageName)}.{rosType.TypeName}";
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}