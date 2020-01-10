using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RosMessageParserCli.CodeGeneration;

namespace RosMessageParserCli
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = LoadConfiguration();

            using (var serviceProvider = CreateContainer(configuration))
            {
                CommandLine.Parser.Default.ParseArguments<CodeGenerationCommandLineOptions>(args)
                    .MapResult(
                        (CodeGenerationCommandLineOptions options) =>
                        {
                            return CodeGeneration.CodeGeneration.Execute(options);
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