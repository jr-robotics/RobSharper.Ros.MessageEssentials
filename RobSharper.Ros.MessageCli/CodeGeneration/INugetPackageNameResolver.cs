using RobSharper.Ros.MessageParser;

namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    public interface INugetPackageNameResolver
    {
        string ResolveNugetPackageName(string rosPackageName);
        
        string ResolveNugetPackageName(RosTypeInfo rosType);
    }
}