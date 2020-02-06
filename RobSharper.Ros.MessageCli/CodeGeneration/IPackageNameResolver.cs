namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    public interface IPackageNameResolver
    {
        string ResolveTypeName(string rosPackageName, string rosTypeName);
    }
}