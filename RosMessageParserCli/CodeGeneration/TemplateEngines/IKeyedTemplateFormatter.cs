namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.TemplateEngines
{
    public interface IKeyedTemplateFormatter
    {
        bool IsAvailable(string templateKey);
        
        string Format(string templateKey, object data);
    }
}