using CommandLine;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    [Verb("generate", HelpText = "Generates code")]
    public class CodeGenerationOptions
    {
        [Option("dll", Required = false, Default = false)]
        public bool CreateDll { get; set; }
        
        [Option("nuget", Required = false, Default = true)]
        public bool CreateNugetPackage { get; set; }
        
        [Option("preserve", Required = false, Default = true, HelpText = "Set to true, if you do not want to delete the source files on successful build.")]
        public bool PreserveGeneratedCode { get; set; }
        
        
        [Value(0, MetaName = "PackagePath", HelpText = "ROS package source folder", Required = true)]
        public string PackagePath { get; set; }
        
        [Value(1, MetaName = "OutputPath", HelpText = "Output path for generated packages", Required = true)]
        public string OutputPath { get; set; }
    }
}