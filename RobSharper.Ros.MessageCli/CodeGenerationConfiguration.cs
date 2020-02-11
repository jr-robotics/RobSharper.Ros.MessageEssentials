using System.Collections.Generic;

namespace RobSharper.Ros.MessageCli
{
    public class CodeGenerationConfiguration
    {
        public string DefaultBuildAction { get; set; }
        public string RootNamespace { get; set; }
        public IList<NugetSourceConfiguration> NugetFeeds { get; set; }
    }
}