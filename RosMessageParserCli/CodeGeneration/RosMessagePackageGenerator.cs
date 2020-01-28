using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.TemplateEngines;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.UmlRobotics;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class RosMessagePackageGenerator
    {
        private readonly CodeGenerationOptions _options;
        private readonly ProjectCodeGenerationDirectoryContext _directories;
        private readonly IKeyedTemplateFormatter _templateEngine;

        private readonly dynamic _data;
        
        private string _projectFilePath;

        private readonly NameMapper _nameMapper;

        public CodeGenerationPackageContext Package { get; }

        public RosMessagePackageGenerator(CodeGenerationPackageContext package, CodeGenerationOptions options,
            ProjectCodeGenerationDirectoryContext directories, IKeyedTemplateFormatter templateEngine)
        {
            Package = package ?? throw new ArgumentNullException(nameof(package));
            
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _directories = directories ?? throw new ArgumentNullException(nameof(directories));
            _templateEngine = templateEngine ?? throw new ArgumentNullException(nameof(templateEngine));

            _nameMapper = new UmlRoboticsNameMapper(Package.PackageInfo.Name,
                new SingleKeyTemplateFormatter(TemplatePaths.PackageName, templateEngine));
            
            _data = new ExpandoObject();
        }
        
        private void EnsurePackageData()
        {
            if (((IDictionary<string, object>)_data).ContainsKey("Package"))
                return;
            
            _data.Package = new ExpandoObject();
            _data.Package.RosName = Package.PackageInfo.Name;
            _data.Package.Version = Package.PackageInfo.Version;
            _data.Package.Name = Package.PackageInfo.Name.ToPascalCase();
            _data.Package.Namespace = _nameMapper.FormatPackageName(Package.PackageInfo.Name);
        }

        public void CreateProject()
        {
            CreateProjectFile();
            AddNugetDependencies();
            
            CreateMessages();
            
            // TODO create other message types
            //CreateServices();
            //CreateActions();
            
            // TODO build project
        }

        private void CreateProjectFile()
        {
            EnsurePackageData();

            var projectFilePath = GetFullQualifiedOutputPath($"{_data.Package.Namespace}.csproj");
            var projectFileContent = _templateEngine.Format(TemplatePaths.ProjectFile, _data.Package);
            WriteFile(projectFilePath, projectFileContent);

            var nugetConfigFilePath = GetFullQualifiedOutputPath("nuget.config");
            var nugetConfigFile = _templateEngine.Format(TemplatePaths.NugetConfigFile, null);
            WriteFile(nugetConfigFilePath, nugetConfigFile);

            _projectFilePath = projectFilePath;
        }

        private void AddNugetDependencies()
        {
            EnsurePackageData();

            var messageNugetPackages = Package.Parser
                .ExternalTypeDependencies
                .Select(x => _nameMapper.ResolveNugetPackageName(x))
                .Distinct()
                .ToList();
                
            // This would be appropriate for Meta Packages
            // var messageNugetPackages = Package.Parser
            //     .PackageDependencies
            //     .Select(x => _messagePackageResolver.ResolveNugetPackageName(x))
            //     .Distinct()
            //     .ToList();
            
            foreach (var dependency in messageNugetPackages)
            {
                var command = $"add \"{_projectFilePath}\" package {dependency}";
                var process = RunDotNet(command);
            }
        }

        private void CreateMessages()
        {
            foreach (var message in Package.Parser.Messages)
            {
                CreateMessage(message.Key, message.Value);
            }
        }
        
        private void CreateMessage(RosTypeInfo rosType, MessageDescriptor message)
        {
            if (_nameMapper.IsBuiltInType(rosType))
                return;

            
            var fields = message.Fields
                .Select(x => new
                {
                    Index = message.Items
                                .Select((item, index) => new {Item = item, Index = index})
                                .First(f => f.Item == x)
                                .Index + 1, // Index of this field in serialized message (starting at 1)
                    RosType = x.TypeInfo,
                    RosIdentifier = x.Identifier,
                    Type = new
                    {
                        InterfaceName = _nameMapper.ResolveInterfacedTypeName(x.TypeInfo),
                        ConcreteName = _nameMapper.ResolveConcreteTypeName(x.TypeInfo),
                        IsBuiltInType = x.TypeInfo.IsBuiltInType,
                        IsArray = x.TypeInfo.IsArray,
                        IsValueType = x.TypeInfo.IsValueType(),
                        SupportsEqualityComparer = x.TypeInfo.SupportsEqualityComparer()
                    },
                    Identifier = x.Identifier.ToPascalCase()
                })
                .ToList();

            var constants = message.Constants
                .Select(c => new
                {
                    Index = message.Items
                                .Select((item, index) => new {item, index})
                                .First(x => x.item == c)
                                .index + 1,
                    RosType = c.TypeInfo,
                    RosIdentifier = c.Identifier,
                    TypeName = _nameMapper.ResolveConcreteTypeName(c.TypeInfo),
                    Identifier = c.Identifier,
                    Value = c.Value
                })
                .ToList();

            var className = rosType.TypeName.ToPascalCase();
            var data = new
            {
                Package = _data.Package,
                RosName = rosType.TypeName,
                Name = className,
                Fields = fields,
                Constants = constants
            };
            
            var fileName = $"{className}.cs";
            var content = _templateEngine.Format(TemplatePaths.MessageFile, data);

            WriteFile(fileName, content);
        }


        private void CreateServices()
        {
            foreach (var service in Package.Parser.Services)
            {
                CreateService(service.Key, service.Value);
            }
        }
        
        private void CreateService(RosTypeInfo rosType, ServiceDescriptor service)
        {
            throw new NotImplementedException();
        }

        

        private void CreateActions()
        {
            foreach (var action in Package.Parser.Actions)
            {
                CreateAction(action.Key, action.Value);
            }
        }
        
        private void CreateAction(RosTypeInfo rosType, ActionDescriptor action)
        {
            throw new NotImplementedException();
        }

        private void WriteFile(string filePath, string content)
        {
            filePath = GetFullQualifiedOutputPath(filePath);
            File.WriteAllText(filePath, content);
        }

        private string GetFullQualifiedOutputPath(string path)
        {
            if (!Path.IsPathFullyQualified(path))
            {
                path = Path.Combine(_directories.OutputDirectory.FullName, path);
            }
            
            return path;
        }

        private static Process RunDotNet(string command)
        {
            const string programName = "dotnet";

            var proc = new Process
            {
                StartInfo =
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = programName,
                    Arguments = command
                }
            };
            
            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                var line = proc.StandardOutput.ReadLine();
                Console.WriteLine(line);
            }

            proc.WaitForExit();
            return proc;
        }
    }
}