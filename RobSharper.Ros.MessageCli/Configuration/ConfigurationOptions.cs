using System;
using CommandLine;

namespace RobSharper.Ros.MessageCli.Configuration
{
    [Verb("config", HelpText = "View or edit configuration options.")]
    public class ConfigurationOptions
    {
        public enum Commands
        {
            Show,
            Set,
            Add,
            Remove
        }

        public enum ConfigurationElements
        {
            DefaultOutput,
            Namespace,
            Feeds
        }
        
        private string _configurationElementString;
        private string _commandString;

        [Value(1, MetaName = "Configuration element",
            HelpText = @"Configuration element: namespace | defaultOutput | feeds
  namespace:       The root namespace for generated message packages
  defaultOutput:   Set the default output format ('nupkg' or 'dll')
  feeds:           A list of nuget feeds used for loading package dependencies",
            Required = true)]
        public string ConfigurationElementString
        {
            get => _configurationElementString;
            set
            {
                _configurationElementString = value;

                if (!Enum.TryParse(typeof(ConfigurationElements), value, true, out var configElement))
                {
                    throw new NotSupportedException($"Configuration element {value} is not supported");
                };

                ConfigurationElement = (ConfigurationElements) configElement;
            }
        }

        public ConfigurationElements ConfigurationElement { get; set; }

        [Value(2, MetaName = "Command", HelpText = "show (default) | set | add | remove", Required = false)]
        public string CommandString
        {
            get => _commandString;
            set
            {
                _commandString = value;

                if (!Enum.TryParse(typeof(Commands), value, true, out var configElement))
                {
                    throw new NotSupportedException($"Configuration element {value} is not supported");
                };

                Command = (Commands) configElement;
            }
        }

        public Commands Command { get; set; }
        
        
        [Value(3, MetaName = "Value", HelpText = "The value to set", Required = false)]
        public string Value { get; set; }

        
        [Option("name", Required = false, HelpText = "Name of the element")]
        public string Name { get; set; }
        
        [Option("source", Required = false, HelpText = "Nuget feed source")]
        public string Source { get; set; }
        
        [Option("protocol", Required = false, HelpText = "Nuget protocol version (e.g. 3)")]
        public int ProtocolVersion { get; set; }
    }
}