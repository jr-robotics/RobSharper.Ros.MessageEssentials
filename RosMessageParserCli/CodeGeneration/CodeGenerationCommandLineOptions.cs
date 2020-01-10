using CommandLine;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    [Verb("generate", HelpText = "Generates code")]
    public class CodeGenerationCommandLineOptions
    {
        private string RosPackagePath { get; set; }
    }
}