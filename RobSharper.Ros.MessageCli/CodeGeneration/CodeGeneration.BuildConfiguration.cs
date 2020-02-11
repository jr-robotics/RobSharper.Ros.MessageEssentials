using System;

namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    public static partial class CodeGeneration
    {
        [Obsolete("Use CodeGenerationConfiguration", true)]
        public class BuildConfiguration
        {
            public string DefaultBuildOption { get; set; }
        }
    }
}