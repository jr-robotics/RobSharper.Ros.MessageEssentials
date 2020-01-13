using System;
using System.IO;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class CodeGenerationDirectoryContext : IDisposable
    {
        private readonly bool _preserveGeneratedCode;
        private bool _disposed;

        public DirectoryInfo TempDirectory { get; }

        public DirectoryInfo OutputDirectory { get; }
        

        public CodeGenerationDirectoryContext(RosPackageInfo packageInfo, string outputPath,
            bool preserveGeneratedCode)
        {
            if (packageInfo == null) throw new ArgumentNullException(nameof(packageInfo));
            if (outputPath == null) throw new ArgumentNullException(nameof(outputPath));
            _preserveGeneratedCode = preserveGeneratedCode;

            var tempPath = GetTempPath(packageInfo);
            TempDirectory = new DirectoryInfo(tempPath);

            if (TempDirectory.Exists)
                TempDirectory.Delete(true);

            TempDirectory.Create();
            
            
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
                    TempDirectory.Delete(true);
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
        
        public static string BasePath { get; } = Path.Combine(Path.GetTempPath(), "RosMessageParser");
        
        public static string GetTempPath(RosPackageInfo packageInfo)
        {
            var packageSlug = $"{packageInfo.Name}__{packageInfo.Version}"
                .Replace('.', '_')
                .ToLowerInvariant();

            return Path.Combine(BasePath, packageSlug);
        }
    }
}