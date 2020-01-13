using System;
using System.Reflection;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public static class CodeGeneration
    {
        public static int Execute(CodeGenerationOptions options)
        {
            var context = CodeGenerationContext.Create(options.PackagePath);

            using (var tempDir = new CodeGenerationDirectoryContext(options.OutputPath, options.PreserveGeneratedCode))
            {
                foreach (var package in context.Packages)
                {
                    // Create Package
                    var packageTempDir = tempDir.GetPackageTempDir(package.PackageInfo);
                    var generator = new RosMessagePackageGenerator(package, options, packageTempDir);

                    generator.CreateProjectFile();
                    generator.BuildMessages();

                    generator.BuildProject();
                    generator.CopyResultsToOutput();
                }
            }

            return 0;
        }
    }
}