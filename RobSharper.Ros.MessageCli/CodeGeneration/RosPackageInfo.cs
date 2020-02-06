using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using RobSharper.Ros.PackageXml;

namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    public class RosPackageInfo
    {
        public string Name { get; }
        public string Version { get; }
        public bool IsMetaPackage { get; }

        private readonly List<string> _packageDependencies;

        private IEnumerable<FileInfo> _messages;

        public DirectoryInfo PackageDirectory { get; }

        public IList<string> PackageDependencies => _packageDependencies;

        public bool HasMessages => Messages.Any();

        public IEnumerable<FileInfo> Messages
        {
            get
            {
                if (_messages != null)
                {
                    return _messages;
                }

                _messages = PackageDirectory
                    .GetDirectories()
                    .Where(d => new[] {"msg", "srv", "action"}.Contains(d.Name.ToLowerInvariant()))
                    .SelectMany(d => d.GetFiles())
                    .Where(f => f.GetRosMessageType() != RosMessageType.None)
                    .ToList();

                return _messages;
            }
        }

        public RosPackageInfo(DirectoryInfo packageDirectory, string name, string version, IEnumerable<string> packageDependencies, bool isMetaPackage)
        {
            PackageDirectory = packageDirectory ?? throw new ArgumentNullException(nameof(packageDirectory));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Version = version ?? throw new ArgumentNullException(nameof(version));
            IsMetaPackage = isMetaPackage;

            _packageDependencies = new List<string>();
            
            if (packageDependencies != null)
                _packageDependencies.AddRange(packageDependencies);
        }


        public static bool IsPackageFolder(string packageFolder)
        {
            // folder is package folder if it contains a package.xml file
            return File.Exists(Path.Combine(packageFolder, "package.xml"));
        }

        public static RosPackageInfo Create(string packageRootPath)
        {
            if (packageRootPath == null) throw new ArgumentNullException(nameof(packageRootPath));
            
            var logger = LoggingHelper.Factory.CreateLogger<RosPackageInfo>();
            
            packageRootPath = Path.GetFullPath(packageRootPath);
            var packageXmlPath = Path.Combine(packageRootPath, "package.xml");

            using (logger.BeginScope($"package.xml ({packageXmlPath})"))
            {
                if (!File.Exists(packageXmlPath))
                {
                    logger.LogError("package.xml not found");
                    throw new FileNotFoundException("package.xml not found");
                }

                try
                {
                    var formatVersion = PackageXmlReader.GetFormatVersion(packageXmlPath);
                    
                    IEnumerable<string> dependentPackages;
                    string name;
                    string packageVersion;
                    bool isMetaPackage;

                    switch (formatVersion)
                    {
                        case 1:
                            var v1Package = PackageXmlReader.ReadV1PackageXml(packageXmlPath);

                            name = v1Package.name;
                            packageVersion = v1Package.version;
                            
                            dependentPackages = v1Package.Items
                                .Select(x => x.Value)
                                .Where(x => x != null);
                            
                            isMetaPackage = v1Package.export?.Any != null && v1Package.export.Any.Any(x =>
                                                "metapackage".Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));
                            
                            break;
                        case 2:
                            var v2Package = PackageXmlReader.ReadV2PackageXml(packageXmlPath);
                            
                            name = v2Package.name;
                            packageVersion = v2Package.version;
                            
                            dependentPackages = v2Package.Items
                                .Select(x => x.Value)
                                .Where(x => x != null);
                            
                            isMetaPackage = v2Package.export?.Any != null && v2Package.export.Any.Any(x =>
                                                "metapackage".Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));
                            
                            break;
                        case 3:
                            var v3Package = PackageXmlReader.ReadV3PackageXml(packageXmlPath);

                            name = v3Package.name;
                            packageVersion = v3Package.version;
                            
                            dependentPackages = v3Package.Items
                                .Select(x => x.Value)
                                .Where(x => x != null);
                            
                            isMetaPackage = v3Package.export?.Any != null && v3Package.export.Any.Any(x =>
                                                "metapackage".Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));
                            
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                   
                    var packageDirectory = new DirectoryInfo(packageRootPath);
                    return new RosPackageInfo(packageDirectory, name, packageVersion, dependentPackages, isMetaPackage);
                }
                catch (Exception e)
                {
                    logger.LogError("Could not deserialize package.xml", e);
                    throw;
                }
            }
        }
    }
}