using System;

namespace RobSharper.Ros.MessageCli.CodeGeneration.TemplateEngines
{
    public class SingleKeyTemplateFormatter : ITemplateFormatter
    {
        private readonly string _key;
        private readonly IKeyedTemplateFormatter _keyedTemplateFormatter;

        public SingleKeyTemplateFormatter(string key, IKeyedTemplateFormatter keyedTemplateFormatter)
        {
            _key = key ?? throw new ArgumentNullException(nameof(key));
            _keyedTemplateFormatter = keyedTemplateFormatter ?? throw new ArgumentNullException(nameof(keyedTemplateFormatter));
        }
        public string Format(object data)
        {
            return _keyedTemplateFormatter.Format(_key, data);
        }
    }
}