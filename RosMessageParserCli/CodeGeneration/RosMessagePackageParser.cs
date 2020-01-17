using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class RosMessagePackageParser : IRosMessagePackageParser
    {
        private IEnumerable<string> _packageDependencies;
        private List<Tuple<string, string>> _externalTypeDependencies;

        private IEnumerable<KeyValuePair<string, MessageDescriptor>> _messages;
        private IEnumerable<KeyValuePair<string, ActionDescriptor>> _actions;
        private IEnumerable<KeyValuePair<string, ServiceDescriptor>> _services;
        
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
        
        public IEnumerable<Tuple<string, string>> ExternalTypeDependencies
        {
            get
            {
                ParseMessages();
                return _externalTypeDependencies;
            }
        }

        public IEnumerable<KeyValuePair<string, MessageDescriptor>> Messages
        {
            get
            {
                ParseMessages();
                return _messages;
            }
        }

        public IEnumerable<KeyValuePair<string, ActionDescriptor>> Actions
        {
            get
            {
                ParseMessages();
                return _actions;
            }
        }

        public IEnumerable<KeyValuePair<string, ServiceDescriptor>> Services
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
            var packageDependencyCollector = new PackageDependencyCollector();
            var typeDependencyCollector = new TypeDependencyCollector();

            var collectors = new RosMessageVisitorListenerCollection(new IRosMessageVisitorListener[]
                {packageDependencyCollector, typeDependencyCollector});

            var messages = new List<KeyValuePair<string, MessageDescriptor>>();
            var actions = new List<KeyValuePair<string, ActionDescriptor>>();
            var services = new List<KeyValuePair<string, ServiceDescriptor>>();

            foreach (var messageFile in Package.Messages)
            {
                using (var file = File.OpenRead(messageFile.FullName))
                {
                    switch (messageFile.GetRosMessageType())
                    {
                        case RosMessageType.Message:
                            var messageParser = new MessageParser(file);
                            var messageDescriptor = messageParser.Parse(collectors);

                            messages.Add(
                                new KeyValuePair<string, MessageDescriptor>(messageFile.Name, messageDescriptor));
                            break;
                        case RosMessageType.Service:
                            var serviceParser = new ServiceParser(file);
                            var serviceDescriptor = serviceParser.Parse(collectors);

                            services.Add(
                                new KeyValuePair<string, ServiceDescriptor>(messageFile.Name, serviceDescriptor));
                            break;
                        case RosMessageType.Action:
                            var actionParser = new ActionParser(file);
                            var actionDescriptor = actionParser.Parse(collectors);

                            actions.Add(
                                new KeyValuePair<string, ActionDescriptor>(messageFile.Name, actionDescriptor));
                            break;
                    }
                }
            }

            var packageDependencies = packageDependencyCollector.Dependencies
                .Where(x => x != Package.Name)
                .ToList();
            
            var typeDependencies = typeDependencyCollector.Dependencies.Values
                .Where(x => x.Item1 != Package.Name)
                .ToList();

            // Add actionlib dependency
            if (actions.Any() && !packageDependencies.Contains("actionlib_msgs"))
            {
                packageDependencies.Add("actionlib_msgs");
            }

            _packageDependencies = packageDependencies;
            _externalTypeDependencies = typeDependencies;
            _messages = messages;
            _actions = actions;
            _services = services;
        }
    }
}