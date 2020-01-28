using System;
using System.Collections.Generic;
using System.Linq;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class RosMetaPackageParser : IRosMessagePackageParser
    {
        private readonly RosPackageInfo _rosPackageInfo;
        private readonly IBuildPackages _buildPackages;
        private bool _done;
        
        private ISet<string> _dependencies;

        public RosMetaPackageParser(RosPackageInfo rosPackageInfo, IBuildPackages buildPackages)
        {
            _rosPackageInfo = rosPackageInfo ?? throw new ArgumentNullException(nameof(rosPackageInfo));
            _buildPackages = buildPackages ?? throw new ArgumentNullException(nameof(buildPackages));
        }

        public RosPackageInfo Package => _rosPackageInfo;

        public IEnumerable<string> PackageDependencies
        {
            get
            {
                ParseMessages();
                return _dependencies;
            }
        }

        public IEnumerable<RosTypeInfo> ExternalTypeDependencies =>
            Enumerable.Empty<RosTypeInfo>();

        public IEnumerable<KeyValuePair<RosTypeInfo, MessageDescriptor>> Messages =>
            Enumerable.Empty<KeyValuePair<RosTypeInfo, MessageDescriptor>>();

        public IEnumerable<KeyValuePair<RosTypeInfo, ActionDescriptor>> Actions =>
            Enumerable.Empty<KeyValuePair<RosTypeInfo, ActionDescriptor>>();

        public IEnumerable<KeyValuePair<RosTypeInfo, ServiceDescriptor>> Services =>
            Enumerable.Empty<KeyValuePair<RosTypeInfo, ServiceDescriptor>>();
        
        public void ParseMessages()
        {
            if (_done)
                return;

            // Find dependent packages based on packages.xml
            var dependencies = new HashSet<string>();

            foreach (var dependency in _rosPackageInfo.PackageDependencies)
            {
                // Add all packages ending with "msgs" (ROS convention)
                // AND
                // Add all other packages set as dependency which are also in the build pipeline
                if (dependency.EndsWith("msgs", StringComparison.InvariantCultureIgnoreCase) ||
                    _buildPackages.Packages.Any(x => x.Name.Equals(dependency, StringComparison.InvariantCultureIgnoreCase)))
                {
                    dependencies.Add(dependency);
                }
            }
            
            _dependencies = dependencies;
            _done = true;
        }
    }
}