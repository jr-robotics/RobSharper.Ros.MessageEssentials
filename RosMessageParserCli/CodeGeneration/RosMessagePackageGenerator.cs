using System;
using System.Collections.Generic;
using System.IO;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class RosMessagePackageGenerator
    {
        private readonly CodeGenerationOptions _options;
        private readonly ProjectCodeGenerationDirectoryContext _codeGenerationDir;
        
        public CodeGenerationPackageContext Package { get; }

        public RosMessagePackageGenerator(CodeGenerationPackageContext package, CodeGenerationOptions options,
            ProjectCodeGenerationDirectoryContext codeGenerationDir)
        {
            Package = package ?? throw new ArgumentNullException(nameof(package));
            
            _options = options;
            _codeGenerationDir = codeGenerationDir;
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