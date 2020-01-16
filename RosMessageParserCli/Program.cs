using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CommandLine;
using HandlebarsDotNet;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.TemplateEngines;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.UmlRobotics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Joanneum.Robotics.Ros.MessageParser.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = LoadConfiguration();

            using (var serviceProvider = CreateContainer(configuration))
            {
                LoggingHelper.Factory = serviceProvider.Resolve<ILoggerFactory>();
                
                CommandLine.Parser.Default.ParseArguments<CodeGenerationOptions>(args)
                    .MapResult(
                        (CodeGenerationOptions options) =>
                        {
                            var templateEngine = serviceProvider.Resolve<IKeyedTemplateFormatter>();
                            return CodeGeneration.CodeGeneration.Execute(options, templateEngine);
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
                        ThrowOnUnresolvedBindingExpression = true
                    };
                    
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
}