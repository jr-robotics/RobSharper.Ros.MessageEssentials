using RobSharper.Ros.MessageParser;

namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    public interface IBuiltInTypeChecker
    {
        bool IsBuiltInType(RosTypeInfo rosType);
    }
}