using RobSharper.Ros.MessageCli.CodeGeneration.MessagePackage;
using RobSharper.Ros.MessageCli.CodeGeneration.MetaPackage;

namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    internal class RosMessageParserFactory
    {
        public IRosMessagePackageParser Create(RosPackageInfo rosPackageInfo, CodeGenerationContext context)
        {
            if (rosPackageInfo.IsMetaPackage)
            {
                return new RosMetaPackageParser(rosPackageInfo, context);
            }
            else
            {
                return new RosMessagePackageParser(rosPackageInfo, context);
            }
        }
    }
}