using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class CodeGenerationContext : IBuildPackages
    {
        public IEnumerable<CodeGenerationPackageContext> Packages { get; private set; }

        private PackageRegistry _packageRegistry;

        public PackageRegistry PackageRegistry
        {
            get
            {
                if (_packageRegistry == null)
                {
                    _packageRegistry = new PackageRegistry(this);
                }

                return _packageRegistry;
            }
        }

        IEnumerable<RosPackageInfo> IBuildPackages.Packages
        {
            get { return Packages?.Select(x => x.PackageInfo); }
        }

        private CodeGenerationContext(IEnumerable<RosPackageInfo> packageInfos)
        {
            if (packageInfos == null) throw new ArgumentNullException(nameof(packageInfos));

            var context = this;
            var factory = new RosMessageParserFactory();
            
            Packages = packageInfos
                .Select(p => new CodeGenerationPackageContext(context, p, factory.Create(p, context)))
                .ToList();
        }

        /// <summary>
        /// Parses the message files of all packages
        /// </summary>
        public void ParseMessages()
        {
            foreach (var package in Packages)
            {
                package.Parser.ParseMessages();
            }
        }

        /// <summary>
        /// Reorders the package list according to build dependencies.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if packages no build sequence without breaking dependencies can be found.</exception>
        public void ReorderPackagesForBuilding()
        {
            ParseMessages();
            
            var buildQueue = new Queue<CodeGenerationPackageContext>();
            while (buildQueue.Count != Packages.Count())
            {
                var packageEnqueued = false;

                foreach (var package in Packages)
                {
                    var dependencies = package.Parser.PackageDependencies;

                    // Package can be built if all dependencies are
                    //    external dependencies (not in build pipeline) OR
                    //    have to be built but are already enqueued
                    if (dependencies.All(x =>
                        !PackageRegistry.Items[x].IsInBuildPipeline || buildQueue.Any(q => q.PackageInfo.Name == x)))
                    {
                        buildQueue.Enqueue(package);
                        packageEnqueued = true;
                    }
                }

                // If no package was enqueued in one round, we cannot build
                if (!packageEnqueued)
                    throw new InvalidOperationException("Can not identify build sequence. All remaining packages have dependencies.");
            }

            Packages = buildQueue;
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