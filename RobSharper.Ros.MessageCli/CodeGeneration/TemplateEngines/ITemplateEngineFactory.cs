namespace RobSharper.Ros.MessageCli.CodeGeneration.TemplateEngines
{
    public interface ITemplateEngineFactory
    {
        IKeyedTemplateFormatter CreateTemplateEngine();
    }
}