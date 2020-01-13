using System;
using System.Reflection;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public static class CodeGeneration
    {
        public static int Execute(CodeGenerationOptions options)
        {
            var context = CodeGenerationContext.Create(options.PackagePath);

            foreach (var package in context.Packages)
            {
                // Create Package
                using (var tempDir = new CodeGenerationDirectoryContext(package, options.OutputPath, options.PreserveGeneratedCode))
                {
                    var generator = new RosMessagePackageGenerator(package, options, tempDir);
                    
                    generator.CreateProjectFile();
                    generator.BuildMessages();

                    generator.BuildProject();
                    generator.CopyResultsToOutput();
                }
            }
            
            return 0;
        }
    }

    public class RosMessagePackageGenerator
    {
        private readonly CodeGenerationOptions _options;
        private readonly CodeGenerationDirectoryContext _codeGenerationDir;
        public RosPackageInfo Package { get; }

        public RosMessagePackageGenerator(RosPackageInfo package, CodeGenerationOptions options,
            CodeGenerationDirectoryContext codeGenerationDir)
        {
            _options = options;
            _codeGenerationDir = codeGenerationDir;
            Package = package ?? throw new ArgumentNullException(nameof(package));
        }

        public void CreateProjectFile()
        {
            throw new NotImplementedException();
        }

        public void BuildMessages()
        {
            throw new NotImplementedException();
        }

        public void BuildProject()
        {
            throw new NotImplementedException();
        }

        public void CopyResultsToOutput()
        {
            throw new NotImplementedException();
        }
    }
}