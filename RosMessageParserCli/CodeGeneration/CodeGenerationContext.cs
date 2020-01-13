using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class CodeGenerationContext
    {
        public IEnumerable<CodeGenerationPackageContext> Packages { get; }
        
        public PackageRegistry PackageRegistry { get; }
        
        private CodeGenerationContext(IEnumerable<RosPackageInfo> packageInfos)
        {
            if (packageInfos == null) throw new ArgumentNullException(nameof(packageInfos));

            Packages = packageInfos
                .Select(p => new CodeGenerationPackageContext(this, p, new RosMessagePackageParser(p)))
                .ToList();
            
            PackageRegistry = new PackageRegistry(this);
        }

        public static CodeGenerationContext Create(string packageFolder)
        {
            if (packageFolder == null) throw new ArgumentNullException(nameof(packageFolder));

            packageFolder = Path.GetFullPath(packageFolder);
            
            if (!Directory.Exists(packageFolder))
            {
                throw new DirectoryNotFoundException($"Directory {packageFolder} does not exit.");
            }

            var packageFolders = FindPackageFolders(packageFolder);
            var packages = packageFolders
                .Select(RosPackageInfo.Create)
                .Where(p => p.IsMetaPackage || p.HasMessages);
            
            var context = new CodeGenerationContext(packages);

            return context;
        }
        
        private static IEnumerable<string> FindPackageFolders(string packageFolder)
        {
            var packageFolders = new List<string>();
            
            if (RosPackageInfo.IsPackageFolder(packageFolder))
            {
                packageFolders.Add(packageFolder);
            }
            else
            {
                foreach (var directory in Directory.GetDirectories(packageFolder))
                {
                    packageFolders.AddRange(FindPackageFolders(directory));
                }
            }

            return packageFolders;
        }
    }
}