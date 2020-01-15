using System.IO;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.UmlRobotics
{
    public static class TemplatePaths
    {
        public static readonly string TemplatesDirectory = Path.Combine("CodeGeneration", "UmlRobotics", "TemplateFiles");

        public const string AssemblyName = "AssemblyName.hbs";
        public const string ProjectFile = "csproj.hbs";
        public const string MessageFile = "Message.cs.hbs";
    }
}