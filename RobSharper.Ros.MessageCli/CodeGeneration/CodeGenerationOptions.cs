using System;
using CommandLine;

namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    [Verb("build", HelpText = "Generates .Net messages from ROS message packages")]
    public class CodeGenerationOptions
    {
        [Option("dll", Required = false, HelpText = "Create DLL")]
        public bool CreateDll { get; set; }
        
        [Option("nupkg", Required = false, HelpText = "Create nuget package")]
        public bool CreateNugetPackage { get; set; }
        
        [Option("preserve", Required = false, HelpText = "Preserve generated source code")]
        public bool PreserveGeneratedCode { get; set; }
        
        
        [Value(0, MetaName = "PackagePath", HelpText = "ROS package(s) source folder", Required = true)]
        public string PackagePath { get; set; }
        
        [Value(1, MetaName = "OutputPath", HelpText = "Output path for generated packages", Required = true)]
        public string OutputPath { get; set; }

        public void SetDefaultBuildAction(CodeGeneration.BuildConfiguration configuration)
        {
            if (CreateDll || CreateNugetPackage || configuration == null)
                return;

            if ("nupkg".Equals(configuration.DefaultBuildOption, StringComparison.InvariantCultureIgnoreCase))
            {
                CreateNugetPackage = true;
            }
            else if ("dll".Equals(configuration.DefaultBuildOption, StringComparison.InvariantCultureIgnoreCase))
            {
                CreateDll = true;
            }
        }
    }
}