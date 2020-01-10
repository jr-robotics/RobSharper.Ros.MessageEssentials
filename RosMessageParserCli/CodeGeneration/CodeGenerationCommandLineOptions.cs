using CommandLine;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    [Verb("generate", HelpText = "Generates code")]
    public class CodeGenerationCommandLineOptions
    {
        [Value(0, MetaName = "PackageFolder", HelpText = "ROS package source folder", Required = true)]
        public string PackageFolder { get; set; }
    }
}