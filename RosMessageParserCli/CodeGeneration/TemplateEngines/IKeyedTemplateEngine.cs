namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.TemplateEngines
{
    public interface IKeyedTemplateEngine : IKeyedTemplateFormatter
    {
        void RegisterTemplateSource(string key, string templateSource);
    }
}