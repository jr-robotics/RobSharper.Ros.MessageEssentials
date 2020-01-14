using System.IO;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class ProjectCodeGenerationDirectoryContext
    {
        public DirectoryInfo OutputDirectory { get; }
        public DirectoryInfo TempDirectory { get; }

        public ProjectCodeGenerationDirectoryContext(DirectoryInfo outputDirectory, DirectoryInfo tempDirectory)
        {
            OutputDirectory = outputDirectory;
            TempDirectory = tempDirectory;
        }
    }
}