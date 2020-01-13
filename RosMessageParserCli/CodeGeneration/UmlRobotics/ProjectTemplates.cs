using System.IO;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.UmlRobotics
{
    public class ProjectTemplates
    {
        private static string _projectFile;

        public static string ProjectFile
        {
            get
            {
                if (_projectFile != null)
                    return _projectFile;

                _projectFile = ReadTemplateFile("csproj.hbs");
                return _projectFile;
            }
        }

        private static string ReadTemplateFile(string teplatePath)
        {
            return File.ReadAllText(teplatePath);
        }
    }
}