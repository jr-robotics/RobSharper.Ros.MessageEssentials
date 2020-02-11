using System;
using System.Collections.Generic;
using CommandLine;

namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    [Verb("build", HelpText = "Generates .Net messages from ROS message packages")]
    public class CodeGenerationOptions
    {
        private string _rootNamespace;

        [Option("dll", Required = false, HelpText = "Create DLL")]
        public bool CreateDll { get; set; }
        
        [Option("nupkg", Required = false, HelpText = "Create nuget package")]
        public bool CreateNugetPackage { get; set; }
        
        [Option("preserve", Required = false, HelpText = "Preserve generated source code")]
        public bool PreserveGeneratedCode { get; set; }

        [Option("root-namespace", Required = false, HelpText = "Root namespace", Hidden = true)]
        public string RootNamespace
        {
            get => _rootNamespace;
            set => _rootNamespace = value?.Trim().TrimEnd('.');
        }


        [Value(0, MetaName = "PackagePath", HelpText = "ROS package(s) source folder", Required = true)]
        public string PackagePath { get; set; }
        
        [Value(1, MetaName = "OutputPath", HelpText = "Output path for generated packages", Required = true)]
        public string OutputPath { get; set; }

        public IEnumerable<string> NugetFeedXmlSources { get; set; }

        public void SetDefaultBuildAction(string defaultBuildOption)
        {
            if (CreateDll || CreateNugetPackage || defaultBuildOption == null)
                return;

            if ("nupkg".Equals(defaultBuildOption, StringComparison.InvariantCultureIgnoreCase))
            {
                CreateNugetPackage = true;
            }
            else if ("dll".Equals(defaultBuildOption, StringComparison.InvariantCultureIgnoreCase))
            {
                CreateDll = true;
            }
        }

        public void SetDefaultRootNamespace(string template)
        {
            if (string.IsNullOrEmpty(RootNamespace))
                RootNamespace = template;
        }
    }
}