using RobSharper.Ros.MessageParser;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public interface IBuiltInTypeChecker
    {
        bool IsBuiltInType(RosTypeInfo rosType);
    }
}