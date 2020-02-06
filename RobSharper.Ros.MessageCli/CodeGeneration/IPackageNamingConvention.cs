namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    public interface IPackageNamingConvention
    {
        string FormatPackageName(string rosPackageName);
    }
}