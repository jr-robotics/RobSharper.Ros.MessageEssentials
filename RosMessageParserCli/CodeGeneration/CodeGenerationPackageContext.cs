using System;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class CodeGenerationPackageContext
    {
        public RosPackageInfo PackageInfo { get; }
        public RosMessagePackageParser Parser { get; }

        public CodeGenerationPackageContext(RosPackageInfo packageInfo, RosMessagePackageParser parser)
        {
            PackageInfo = packageInfo ?? throw new ArgumentNullException(nameof(packageInfo));
            Parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }
    }
}