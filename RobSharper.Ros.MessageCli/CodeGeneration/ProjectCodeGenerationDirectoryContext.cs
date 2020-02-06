using System.IO;

namespace RobSharper.Ros.MessageCli.CodeGeneration
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