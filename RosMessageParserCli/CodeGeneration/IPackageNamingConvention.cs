namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public interface IPackageNamingConvention
    {
        string FormatPackageName(string rosPackageName);
    }
}