using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class CodeGenerationDirectoryContext : IDisposable
    {
        public static string BaseTempPath { get; } = Path.Combine(Path.GetTempPath(), "RobSharper.Ros.MessageGeneration");
        
        private readonly bool _preserveGeneratedCode;
        private bool _disposed;

        private readonly IDictionary<string, ProjectCodeGenerationDirectoryContext> _projectTempDirectories =
            new Dictionary<string, ProjectCodeGenerationDirectoryContext>();

        public DirectoryInfo OutputDirectory { get; }
        
        public DirectoryInfo NugetTempDirectory { get; }
        

        public CodeGenerationDirectoryContext(string outputPath, bool preserveGeneratedCode)
        {
            if (outputPath == null) throw new ArgumentNullException(nameof(outputPath));
            _preserveGeneratedCode = preserveGeneratedCode;
            
            outputPath = Path.GetFullPath(outputPath);
            OutputDirectory = new DirectoryInfo(outputPath);

            if (!OutputDirectory.Exists)
                OutputDirectory.Create();

            var tempSlug = $"x{Guid.NewGuid():N}";
            NugetTempDirectory = new DirectoryInfo(Path.Combine(BaseTempPath, "nuget", tempSlug));
            
            if (!NugetTempDirectory.Exists)
                NugetTempDirectory.Create();
        }
        
        public ProjectCodeGenerationDirectoryContext GetPackageTempDir(RosPackageInfo packageInfo)
        {
            var tempPath = Path.Combine(BaseTempPath, packageInfo.Name, packageInfo.Version);

            if (_projectTempDirectories.TryGetValue(tempPath, out var context)) 
                return context;
            
            var tempDirectory = new DirectoryInfo(tempPath);

            if (tempDirectory.Exists)
                tempDirectory.Delete(true);

            tempDirectory.Create();

            context = new ProjectCodeGenerationDirectoryContext(OutputDirectory, tempDirectory, NugetTempDirectory);
            _projectTempDirectories.Add(tempPath, context);

            return context;
        }

        private void ReleaseUnmanagedResources()
        {
            if (_disposed)
                return;

            try
            {
                var tasks = new List<Task>();
                
                if (!_preserveGeneratedCode)
                {
                    foreach (var directory in _projectTempDirectories)
                    {
                        var task = Task.Factory.StartNew(() => directory.Value.TempDirectory.Delete(true));
                        tasks.Add(task);
                    }
                }

                Task.Factory.StartNew(() => NugetTempDirectory.Delete(true));
                
                Task.WaitAll(tasks.ToArray());
            }
            finally
            {
                _disposed = true;
            }
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~CodeGenerationDirectoryContext()
        {
            ReleaseUnmanagedResources();
        }
    }
}