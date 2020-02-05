using System.Collections.Generic;
using RobSharper.Ros.MessageParser;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public interface IRosMessagePackageParser
    {
        RosPackageInfo Package { get; }
        
        IEnumerable<string> PackageDependencies { get; }
        IEnumerable<RosTypeInfo> ExternalTypeDependencies { get; }
        
        IEnumerable<KeyValuePair<RosTypeInfo, MessageDescriptor>> Messages { get; }
        IEnumerable<KeyValuePair<RosTypeInfo, ActionDescriptor>> Actions { get; }
        IEnumerable<KeyValuePair<RosTypeInfo, ServiceDescriptor>> Services { get; }
        
        void ParseMessages();
    }
}