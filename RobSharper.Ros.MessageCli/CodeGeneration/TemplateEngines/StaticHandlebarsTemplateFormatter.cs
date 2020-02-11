using System;
using HandlebarsDotNet;

namespace RobSharper.Ros.MessageCli.CodeGeneration.TemplateEngines
{
    public class StaticHandlebarsTemplateFormatter : ITemplateFormatter
    {
        private readonly Func<object, string> _template;

        public StaticHandlebarsTemplateFormatter(string template, HandlebarsConfiguration configuration = null)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));
            
            if (configuration == null)
            {
                configuration = new HandlebarsConfiguration()
                {
                    ThrowOnUnresolvedBindingExpression = true
                };
            }

            var handlebars = HandlebarsDotNet.Handlebars.Create(configuration);

            _template = handlebars.Compile(template);
        }
        
        public string Format(object data)
        {
            return _template(data);
        }
    }
}