using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class CodeGenerationDirectoryContext : IDisposable
    {
        private readonly bool _preserveGeneratedCode;
        private bool _disposed;

        private readonly IDictionary<string, ProjectCodeGenerationDirectoryContext> _projectTempDirectories =
            new Dictionary<string, ProjectCodeGenerationDirectoryContext>();

        public DirectoryInfo OutputDirectory { get; }
        

        public CodeGenerationDirectoryContext(string outputPath, bool preserveGeneratedCode)
        {
            if (outputPath == null) throw new ArgumentNullException(nameof(outputPath));
            _preserveGeneratedCode = preserveGeneratedCode;
            
            outputPath = Path.GetFullPath(outputPath);
            OutputDirectory = new DirectoryInfo(outputPath);

            if (!OutputDirectory.Exists)
                OutputDirectory.Create();
        }

        private void ReleaseUnmanagedResources()
        {
            if (_disposed)
                return;

            try
            {
                if (!_preserveGeneratedCode)
                {
                    var tasks = new List<Task>();
                    foreach (var directory in _projectTempDirectories)
                    {
                        var task = Task.Factory.StartNew(() => directory.Value.TempDirectory.Delete(true));
                        tasks.Add(task);
                    }

                    Task.WaitAll(tasks.ToArray());
                }
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
        
        public static string BaseTempPath { get; } = Path.Combine(Path.GetTempPath(), "RobSharper.Ros.MessageGeneration");
        
        public static string GetTempPath(RosPackageInfo packageInfo)
        {
            return Path.Combine(BaseTempPath, packageInfo.Name, packageInfo.Version);
        }

        public ProjectCodeGenerationDirectoryContext GetPackageTempDir(RosPackageInfo packageInfo)
        {
            var tempPath = GetTempPath(packageInfo);

            if (_projectTempDirectories.TryGetValue(tempPath, out var context)) 
                return context;
            
            var tempDirectory = new DirectoryInfo(tempPath);

            if (tempDirectory.Exists)
                tempDirectory.Delete(true);

            tempDirectory.Create();

            context = new ProjectCodeGenerationDirectoryContext(OutputDirectory, tempDirectory);
            _projectTempDirectories.Add(tempPath, context);

            return context;
        }
    }
}