using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HandlebarsDotNet;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.TemplateEngines;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.UmlRobotics;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public static class CodeGeneration
    {
        private static UmlRosPackageNameResolver _packageResolver;

        public static int Execute(CodeGenerationOptions options)
        {
            var context = CodeGenerationContext.Create(options.PackagePath);

            using (var directories = new CodeGenerationDirectoryContext(options.OutputPath, options.PreserveGeneratedCode))
            {
                // Parse message files and build package dependency graph
                context.ParseMessages();
                
                // Set build order depending on package dependencies
                context.ReorderPackagesForBuilding();

                // Check external dependencies
                CheckExternalPackagerDependencies(context);
                
                // Setup template engine
                var templateEngine = CreateTemplateEngine();

                foreach (var package in context.Packages)
                {
                    // Create Package
                    var packageDirectories = directories.GetPackageTempDir(package.PackageInfo);
                    var generator = new RosMessagePackageGenerator(package, options, packageDirectories, templateEngine);

                    generator.CreateProject();
                }
            }

            return 0;
        }

        private static IKeyedTemplateEngine CreateTemplateEngine()
        {
            var templateEngine = new FileBasedHandleBarsTemplateEngine();
            templateEngine.BasePath = Path.Combine(templateEngine.BasePath, TemplatePaths.TemplatesDirectory);
            
            _packageResolver = new UmlRosPackageNameResolver(templateEngine);
            
            templateEngine.Handlebars.RegisterHelper("mapIdentifier", (output, context, arguments) =>
            {
                var identifier = arguments[0].ToString();
                output.WriteSafeString(identifier.ToPascalCase());
            });
            
            templateEngine.Handlebars.RegisterHelper("mapType", (output, context, arguments) =>
            {
                var rosType = (IRosTypeInfo) arguments[0];
                output.WriteSafeString(GetTypeName(rosType));
            });
            
            return templateEngine;
        }

        private static string GetTypeName(IRosArrayTypeInfo type)
        {
            return $"System.Collections.Generic.IList<{GetTypeName(type.GetUnderlyingType())}>";
        }
        
        private static string GetTypeName(IRosTypeInfo type)
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

        private static void CheckExternalPackagerDependencies(CodeGenerationContext context)
        {
            foreach (var package in context.PackageRegistry.Items.Values)
            {
                if (package.IsAvailable)
                    continue;
                
                //TODO: Check nuget repo(s). For now we assume that all packages can be loaded on build 
            }
        }
    }
}