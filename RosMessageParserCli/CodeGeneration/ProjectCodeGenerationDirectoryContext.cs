using System.IO;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class ProjectCodeGenerationDirectoryContext
    {
        public DirectoryInfo OutputDirectory { get; }
        public DirectoryInfo TempDirectory { get; }
        public DirectoryInfo  NugetTempDirectory { get; }

        public ProjectCodeGenerationDirectoryContext(DirectoryInfo outputDirectory, DirectoryInfo tempDirectory, DirectoryInfo nugetTempDirectory)
        {
            OutputDirectory = outputDirectory;
            TempDirectory = tempDirectory;
            NugetTempDirectory = nugetTempDirectory;
        }
    }
}