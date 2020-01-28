using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class RosMessagePackageParser : IRosMessagePackageParser
    {
        private IEnumerable<string> _packageDependencies;
        private IEnumerable<RosTypeInfo> _externalTypeDependencies;

        private IEnumerable<KeyValuePair<RosTypeInfo, MessageDescriptor>> _messages;
        private IEnumerable<KeyValuePair<RosTypeInfo, ActionDescriptor>> _actions;
        private IEnumerable<KeyValuePair<RosTypeInfo, ServiceDescriptor>> _services;
        
        private readonly object _lock = new object();
        private bool _done;

        public RosPackageInfo Package { get; }
        public IBuildPackages BuildPackages { get; }

        public IEnumerable<string> PackageDependencies
        {
            get
            {
                ParseMessages();
                return _packageDependencies;
            }
        }
        
        public IEnumerable<RosTypeInfo> ExternalTypeDependencies
        {
            get
            {
                ParseMessages();
                return _externalTypeDependencies;
            }
        }

        public IEnumerable<KeyValuePair<RosTypeInfo, MessageDescriptor>> Messages
        {
            get
            {
                ParseMessages();
                return _messages;
            }
        }

        public IEnumerable<KeyValuePair<RosTypeInfo, ActionDescriptor>> Actions
        {
            get
            {
                ParseMessages();
                return _actions;
            }
        }

        public IEnumerable<KeyValuePair<RosTypeInfo, ServiceDescriptor>> Services
        {
            get
            {
                ParseMessages();
                return _services;
            }
        }

        public RosMessagePackageParser(RosPackageInfo package, IBuildPackages buildPackages)
        {
            Package = package ?? throw new ArgumentNullException(nameof(package));
            BuildPackages = buildPackages ?? throw new ArgumentNullException(nameof(buildPackages));
        }
        
        public void ParseMessages()
        {
            if (_done)
                return;

            lock (_lock)
            {
                if (_done)
                    return;

                if (Package.IsMetaPackage)
                {
                    throw new NotSupportedException("Meta packages are not supported by this parser");
                }

                ParseMessagesInternal();

                _done = true;
            }
        }

        private void ParseMessagesInternal()
        {
            var packageDependencyCollector = new PackageDependencyCollector(new [] { Package.Name });
            var typeDependencyCollector = new TypeDependencyCollector(new [] { Package.Name });

            var collectors = new RosMessageVisitorListenerCollection(new IRosMessageVisitorListener[]
                {packageDependencyCollector, typeDependencyCollector});

            var messages = new List<KeyValuePair<RosTypeInfo, MessageDescriptor>>();
            var actions = new List<KeyValuePair<RosTypeInfo, ActionDescriptor>>();
            var services = new List<KeyValuePair<RosTypeInfo, ServiceDescriptor>>();

            foreach (var messageFile in Package.Messages)
            {
                using (var fileStream = File.OpenRead(messageFile.FullName))
                {
                    var rosType = RosTypeInfo.CreateRosType(Package.Name, messageFile.NameWithoutExtension());
                    
                    switch (messageFile.GetRosMessageType())
                    {
                        case RosMessageType.Message:
                            var messageParser = new MessageParser(fileStream);
                            var messageDescriptor = messageParser.Parse(collectors);
                            
                            messages.Add(
                                new KeyValuePair<RosTypeInfo, MessageDescriptor>(rosType, messageDescriptor));
                            break;
                        case RosMessageType.Service:
                            var serviceParser = new ServiceParser(fileStream);
                            var serviceDescriptor = serviceParser.Parse(collectors);

                            services.Add(
                                new KeyValuePair<RosTypeInfo, ServiceDescriptor>(rosType, serviceDescriptor));
                            break;
                        case RosMessageType.Action:
                            var actionParser = new ActionParser(fileStream);
                            var actionDescriptor = actionParser.Parse(collectors);

                            actions.Add(
                                new KeyValuePair<RosTypeInfo, ActionDescriptor>(rosType, actionDescriptor));
                            break;
                    }
                }
            }

            // Add actionlib dependency
            if (actions.Any() && !packageDependencyCollector.Dependencies.Contains("actionlib_msgs"))
            {
                packageDependencyCollector.Dependencies.Add("actionlib_msgs");
            }

            _packageDependencies = packageDependencyCollector.Dependencies;
            _externalTypeDependencies = typeDependencyCollector.Dependencies;
            _messages = messages;
            _actions = actions;
            _services = services;
        }
    }
}