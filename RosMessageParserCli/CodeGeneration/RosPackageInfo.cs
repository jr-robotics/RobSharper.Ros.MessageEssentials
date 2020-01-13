using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class RosPackageInfo
    {
        public string Name { get; }
        public string Version { get; }
        public bool IsMetaPackage { get; }

        private readonly List<string> _messagePackageDependencies;

        private IEnumerable<FileInfo> _messages;

        public DirectoryInfo PackageDirectory { get; }

        public IList<string> MessagePackageDependencies => _messagePackageDependencies;

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

        public RosPackageInfo(DirectoryInfo packageDirectory, string name, string version, IEnumerable<string> messagePackageDependencies, bool isMetaPackage)
        {
            PackageDirectory = packageDirectory ?? throw new ArgumentNullException(nameof(packageDirectory));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Version = version ?? throw new ArgumentNullException(nameof(version));
            IsMetaPackage = isMetaPackage;

            _messagePackageDependencies = new List<string>();
            
            if (messagePackageDependencies != null)
                _messagePackageDependencies.AddRange(messagePackageDependencies);
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
                    var serializer = new XmlSerializer(typeof(PackageXml.V2.package));
                    var package = (PackageXml.V2.package) serializer.Deserialize(new XmlTextReader(packageXmlPath));

                    var messagePackages = package.Items?
                        .Where(x => x.Value != null &&
                                    x.Value.EndsWith("msgs", StringComparison.InvariantCultureIgnoreCase))
                        .Select(x => x.Value);

                    var isMetaPackage = package.export?.Any != null && package.export.Any.Any(x =>
                                            "metapackage".Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));
                    
                    var packageDirectory = new DirectoryInfo(packageRootPath);
                    return new RosPackageInfo(packageDirectory, package.name, package.version, messagePackages, isMetaPackage);
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