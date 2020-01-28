namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public interface INugetPackageNameResolver
    {
        string ResolveNugetPackageName(string rosPackageName);
        
        string ResolveNugetPackageName(RosTypeInfo rosType);
    }
}