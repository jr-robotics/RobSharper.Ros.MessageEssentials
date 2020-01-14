namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
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