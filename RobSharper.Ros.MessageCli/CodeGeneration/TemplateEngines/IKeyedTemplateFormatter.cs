namespace RobSharper.Ros.MessageCli.CodeGeneration.TemplateEngines
{
    public interface IKeyedTemplateFormatter
    {
        bool IsAvailable(string templateKey);
        
        string Format(string templateKey, object data);
    }
}