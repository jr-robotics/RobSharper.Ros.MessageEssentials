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
        
        private IEnumerable<string> _dependencies;

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

        public IEnumerable<Tuple<string, string>> ExternalTypeDependencies => 
            Enumerable.Empty<Tuple<string, string>>();

        public IEnumerable<KeyValuePair<string, MessageDescriptor>> Messages =>
            Enumerable.Empty<KeyValuePair<string, MessageDescriptor>>();

        public IEnumerable<KeyValuePair<string, ActionDescriptor>> Actions =>
            Enumerable.Empty<KeyValuePair<string, ActionDescriptor>>();

        public IEnumerable<KeyValuePair<string, ServiceDescriptor>> Services =>
            Enumerable.Empty<KeyValuePair<string, ServiceDescriptor>>();
        
        public void ParseMessages()
        {
            if (_done)
                return;

            // Find dependent packages based on packages.xml
            
            var items = new List<string>();

            foreach (var dependency in _rosPackageInfo.PackageDependencies)
            {
                // Add all packages ending with "msgs" (ROS convention)
                // AND
                // Add all other packages set as dependency which are also in the build pipeline
                if (dependency.EndsWith("msgs", StringComparison.InvariantCultureIgnoreCase) ||
                    _buildPackages.Packages.Any(x => x.Name.Equals(dependency, StringComparison.InvariantCultureIgnoreCase)))
                {
                    items.Add(dependency);
                }
            }
            
            _dependencies = items;
            _done = true;
        }
    }
}