using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CommandLine;
using HandlebarsDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RobSharper.Ros.MessageCli.CodeGeneration;
using RobSharper.Ros.MessageCli.CodeGeneration.TemplateEngines;
using RobSharper.Ros.MessageCli.CodeGeneration.UmlRobotics;
using RobSharper.Ros.MessageCli.Configuration;

namespace RobSharper.Ros.MessageCli
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = LoadConfiguration();

            using (var serviceProvider = CreateContainer(configuration))
            {
                LoggingHelper.Factory = serviceProvider.Resolve<ILoggerFactory>();

                var commandLineParser = new Parser(settings =>
                {
                    settings.HelpWriter = Console.Error;
                    settings.CaseInsensitiveEnumValues = true;
                });
                
                commandLineParser.ParseArguments<CodeGenerationOptions, ConfigurationOptions>(args)
                    .MapResult(
                        (CodeGenerationOptions options) =>
                        {
                            var config = configuration.GetSection("Build");
                            var configObject = new CodeGenerationConfiguration();
                            config.Bind(configObject);
                            
                            options.SetDefaultBuildAction(configObject.DefaultBuildAction);
                            options.SetDefaultRootNamespace(configObject.RootNamespace);
                            options.NugetFeedXmlSources = configObject.NugetFeeds?
                                .Select(f => f.GetXmlString())
                                .ToList() ?? Enumerable.Empty<string>();
                            
                            var templateEngine = serviceProvider.Resolve<IKeyedTemplateFormatter>();
                            return CodeGeneration.CodeGeneration.Execute(options, templateEngine);
                        },
                        (ConfigurationOptions options) =>
                        {
                            return ConfigurationProgram.Execute(options);
                        },
                        errs => 1);
            }
        }

        private static IContainer CreateContainer(IConfiguration configuration)
        {
            IServiceCollection services = new ServiceCollection();
            
            services
                .AddLogging(x => x
                    .AddConfiguration(configuration.GetSection("Logging"))
                    .AddDebug()
                    .AddConsole());
            
            var containerBuilder = new ContainerBuilder();
            
            containerBuilder.Populate(services);

            // Template Engine
            containerBuilder.Register(context =>
                {
                    var config = new HandlebarsConfiguration
                    {
                        ThrowOnUnresolvedBindingExpression = true,
                    };
                    
                    config.Helpers.Add("formatValue", (output, hbContext, arguments) =>
                    {
                        object value = arguments[0];

                        if (value is string)
                        {
                            output.WriteSafeString("\"");
                            output.WriteSafeString(value
                                .ToString()
                                .Replace("\t", "\\\t")
                                .Replace("\"", "\\\"")
                            );
                            output.WriteSafeString("\"");
                            return;
                        }

                        if (value is float)
                        {
                            output.WriteSafeString(value);
                            output.WriteSafeString("f");
                            return;
                        }

                        if (value is bool)
                        {
                            output.WriteSafeString(string.Format(CultureInfo.InvariantCulture, "{0}", value).ToLowerInvariant());
                            return;
                        }
                        
                        output.WriteSafeString(value);
                    });
                    return new FileBasedHandlebarsTemplateEngine(TemplatePaths.TemplatesDirectory, config);
                })
                .SingleInstance()
                .As<IKeyedTemplateEngine>()
                .As<IKeyedTemplateFormatter>();
            
            
            var container = containerBuilder.Build();
            
            return container;
        }
        
        private static IConfigurationRoot LoadConfiguration()
        {
            IConfigurationRoot configuration;

            try
            {
                configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("config.json")
                    .Build();
            }
            catch (Exception)
            {
                Console.WriteLine("Could not load configuration.");
                throw;
            }

            return configuration;
        }
    }
}