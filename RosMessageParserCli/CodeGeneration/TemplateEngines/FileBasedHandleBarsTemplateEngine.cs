using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HandlebarsDotNet;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.TemplateEngines
{
    public class FileBasedHandleBarsTemplateEngine : IKeyedTemplateEngine
    {
        private readonly Dictionary<string, Func<object, string>> _templates = new Dictionary<string, Func<object, string>>();
        private string _basePath;
        private IHandlebars _handlebars;

        public string BasePath
        {
            get
            {
                if (_basePath == null)
                {
                    _basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                }
                return _basePath;
            }
            set { _basePath = value; }
        }

        public HandlebarsConfiguration Configuration { get; }

        public IHandlebars Handlebars
        {
            get
            {
                if (_handlebars == null)
                {
                    _handlebars = HandlebarsDotNet.Handlebars.Create(Configuration);
                }
                return _handlebars;
            }
        }

        public bool Exists(string key)
        {
            return _templates.ContainsKey(key);
        }

        public FileBasedHandleBarsTemplateEngine()
        {
            Configuration = new HandlebarsConfiguration()
            {
                ThrowOnUnresolvedBindingExpression = true
            };
        }

        public bool IsAvailable(string templateKey)
        {
            if (Exists(templateKey)) 
                return true;

            var fullPath = GetFullTemplatePath(templateKey);
            return File.Exists(fullPath);
        }

        public string Format(string fileName, object data)
        {
            if (!Exists(fileName))
            {
                RegisterTemplateFile(fileName);
            }
            
            return _templates[fileName](data);
        }

        public void RegisterTemplateSource(string key, string templateSource)
        {
            var template = Handlebars.Compile(templateSource);
            _templates.Add(key, template);
        }
        
        public void RegisterTemplateFile(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            
            var key = filePath;
            filePath = GetFullTemplatePath(filePath);

            var template = File.ReadAllText(filePath);
            RegisterTemplateSource(key, template);
        }

        private string GetFullTemplatePath(string filePath)
        {
            if (!Path.IsPathFullyQualified(filePath))
            {
                filePath = Path.Combine(BasePath, filePath);
            }

            return filePath;
        }
    }
}