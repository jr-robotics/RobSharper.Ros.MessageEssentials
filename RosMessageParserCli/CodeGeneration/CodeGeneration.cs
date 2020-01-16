using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.TemplateEngines;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public static class CodeGeneration
    {
        public static int Execute(CodeGenerationOptions options, IKeyedTemplateFormatter templateEngine)
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