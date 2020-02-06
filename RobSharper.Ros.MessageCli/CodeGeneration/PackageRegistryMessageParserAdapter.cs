using System;
using System.Collections.Generic;
using RobSharper.Ros.MessageParser;

namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    public class PackageRegistryMessageParserAdapter : IRosMessagePackageParser
    {
        private readonly IPackageRegistry _packageRegistry;
        private readonly IRosMessagePackageParser _innerParser;

        private bool _done;
        private readonly object _lock = new object();

        public PackageRegistryMessageParserAdapter(IPackageRegistry packageRegistry, IRosMessagePackageParser parser)
        {
            _packageRegistry = packageRegistry ?? throw new ArgumentNullException(nameof(packageRegistry));
            _innerParser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        public RosPackageInfo Package => _innerParser.Package;
        
        public IEnumerable<string> PackageDependencies
        {
            get
            {
                ParseMessages();
                return _innerParser.PackageDependencies;
            }
        }

        public IEnumerable<RosTypeInfo> ExternalTypeDependencies
        {
            get
            {
                ParseMessages();
                return _innerParser.ExternalTypeDependencies;
            }
        }

        public IEnumerable<KeyValuePair<RosTypeInfo, MessageDescriptor>> Messages
        {
            get
            {
                ParseMessages();
                return _innerParser.Messages;
            }
        }

        public IEnumerable<KeyValuePair<RosTypeInfo, ActionDescriptor>> Actions
        {
            get
            {
                ParseMessages();
                return _innerParser.Actions;
            }
        }

        public IEnumerable<KeyValuePair<RosTypeInfo, ServiceDescriptor>> Services
        {
            get
            {
                ParseMessages();
                return _innerParser.Services;
            }
        }
        
        public void ParseMessages()
        {
            if (_done)
                return;

            lock (_lock)
            {
                if (_done)
                    return;
                
                _innerParser.ParseMessages();

                foreach (var dependency in _innerParser.PackageDependencies)
                {
                    _packageRegistry.AddDependency(dependency);
                }

                _done = true;
            }
        }
    }
}