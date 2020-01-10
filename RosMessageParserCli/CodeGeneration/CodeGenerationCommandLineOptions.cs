using CommandLine;

namespace RosMessageParserCli.CodeGeneration
{
    [Verb("generate", HelpText = "Generates code")]
    public class CodeGenerationCommandLineOptions
    {
        private string RosPackagePath { get; set; }
    }
}