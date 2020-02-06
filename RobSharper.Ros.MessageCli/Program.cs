using System;
using System.Globalization;
using System.IO;
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
                
                CommandLine.Parser.Default.ParseArguments<CodeGenerationOptions, PlaceholderOptions>(args)
                    .MapResult(
                        (CodeGenerationOptions options) =>
                        {
                            var buildConfig = configuration.GetSection("Build");
                            var templateEngine = serviceProvider.Resolve<IKeyedTemplateFormatter>();
                            return CodeGeneration.CodeGeneration.Execute(options, templateEngine, buildConfig);
                        },
                        (PlaceholderOptions options) =>
                        {
                            return 1;
                        },
                        errs => 1
                    );
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

            
            // Add UML Robotics dependencies
            // Package name resolver
            // containerBuilder.Register(context =>
            //     {
            //         return new UmlRosPackageNameResolver(
            //             new SingleKeyTemplateFormatter(TemplatePaths.PackageName,
            //                 new FileBasedHandlebarsTemplateEngine(TemplatePaths.TemplatesDirectory,
            //                     new HandlebarsConfiguration
            //                         {ThrowOnUnresolvedBindingExpression = true})));
            //     })
            //     .SingleInstance()
            //     .As<IRosPackageNameResolver>();

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

    [Verb("placeholder0815", HelpText = "This is just a placeholder so that build must be set as argument", Hidden = true)]
    public class PlaceholderOptions
    {
    }
}