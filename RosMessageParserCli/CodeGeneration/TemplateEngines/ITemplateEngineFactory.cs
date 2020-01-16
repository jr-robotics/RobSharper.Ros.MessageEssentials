namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.TemplateEngines
{
    public interface ITemplateEngineFactory
    {
        IKeyedTemplateFormatter CreateTemplateEngine();
    }
}