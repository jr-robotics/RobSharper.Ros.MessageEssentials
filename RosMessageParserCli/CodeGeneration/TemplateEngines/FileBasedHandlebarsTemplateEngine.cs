using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HandlebarsDotNet;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.TemplateEngines
{
    public class FileBasedHandlebarsTemplateEngine : IKeyedTemplateEngine
    {
        private readonly Dictionary<string, Func<object, string>> _templates = new Dictionary<string, Func<object, string>>();

        public string BasePath { get; }

        public HandlebarsConfiguration Configuration { get; }

        public IHandlebars Handlebars { get; }

        public bool Exists(string key)
        {
            return _templates.ContainsKey(key);
        }

        public FileBasedHandlebarsTemplateEngine(string templateDirectoryPath)
        {
            BasePath = templateDirectoryPath ?? throw new ArgumentNullException(nameof(templateDirectoryPath));
            
            Configuration = new HandlebarsConfiguration()
            {
                ThrowOnUnresolvedBindingExpression = true
            };
            
            Handlebars = HandlebarsDotNet.Handlebars.Create(Configuration);
        }

        public FileBasedHandlebarsTemplateEngine(string templateDirectoryPath, HandlebarsConfiguration configuration)
        {
            BasePath = templateDirectoryPath ?? throw new ArgumentNullException(nameof(templateDirectoryPath));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Handlebars = HandlebarsDotNet.Handlebars.Create(Configuration);
        }

        public FileBasedHandlebarsTemplateEngine(string templateDirectoryPath, IHandlebars handlebars)
        {
            if (handlebars == null) throw new ArgumentNullException(nameof(handlebars));
            BasePath = templateDirectoryPath ?? throw new ArgumentNullException(nameof(templateDirectoryPath));
            
            Configuration = handlebars.Configuration;
            Handlebars = handlebars;
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