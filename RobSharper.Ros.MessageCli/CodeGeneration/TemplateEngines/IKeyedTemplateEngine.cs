namespace RobSharper.Ros.MessageCli.CodeGeneration.TemplateEngines
{
    public interface IKeyedTemplateEngine : IKeyedTemplateFormatter
    {
        void RegisterTemplateSource(string key, string templateSource);
    }
}