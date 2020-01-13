using System;
using System.Collections.Generic;
using System.Linq;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public static class CodeGeneration
    {
        public static int Execute(CodeGenerationOptions options)
        {
            var context = CodeGenerationContext.Create(options.PackagePath);

            using (var directories = new CodeGenerationDirectoryContext(options.OutputPath, options.PreserveGeneratedCode))
            {
                // Build package dependencyGraph
                foreach (var package in context.Packages)
                {
                    package.Parser.ParseMessages();
                }
                
                // Set build order
                var buildQueue = CreateBuildQueue(context);

                // Check external dependencies
                CheckExternalPackagerDependencies(context);
                
                foreach (var package in context.Packages)
                {
                    // Create Package
                    var packageDirectories = directories.GetPackageTempDir(package.PackageInfo);
                    var generator = new RosMessagePackageGenerator(package, options, packageDirectories);

                    generator.CreateProjectFile();
                    generator.BuildMessages();

                    generator.BuildProject();
                    generator.CopyResultsToOutput();
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
                
                // Check nuget repo!
            }
        }

        private static Queue<CodeGenerationPackageContext> CreateBuildQueue(CodeGenerationContext context)
        {
            var buildQueue = new Queue<CodeGenerationPackageContext>();
            while (buildQueue.Count != context.Packages.Count())
            {
                var packageEnqueued = false;

                foreach (var package in context.Packages)
                {
                    var dependencies = package.Parser.PackageDependencies;

                    // Package can be built if all dependencies are
                    //    external dependencies (not in build pipeline) OR
                    //    have to be built but are already enqueued
                    if (dependencies.All(x =>
                        !context.PackageRegistry.Items[x].IsInBuildPipeline || buildQueue.Any(q => q.PackageInfo.Name == x)))
                    {
                        buildQueue.Enqueue(package);
                        packageEnqueued = true;
                    }
                }

                // If no package was enqueued in one round, we cannot build
                if (!packageEnqueued)
                    throw new Exception(); // TODO
            }

            return buildQueue;
        }
    }
}