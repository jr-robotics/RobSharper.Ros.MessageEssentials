using CommandLine;

namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    [Verb("build", HelpText = "Generates .Net messages from ROS message packages")]
    public class CodeGenerationOptions
    {
        [Option("dll", Required = false, Default = false)]
        public bool CreateDll { get; set; }
        
        [Option("nuget", Required = false, Default = true)]
        public bool CreateNugetPackage { get; set; }
        
        [Option("preserve", Required = false, Default = false, HelpText = "Set to true, if you do not want to delete the source files on successful build.")]
        public bool PreserveGeneratedCode { get; set; }
        
        
        [Value(0, MetaName = "PackagePath", HelpText = "ROS package(s) source folder", Required = true)]
        public string PackagePath { get; set; }
        
        [Value(1, MetaName = "OutputPath", HelpText = "Output path for generated packages", Required = true)]
        public string OutputPath { get; set; }
    }
}