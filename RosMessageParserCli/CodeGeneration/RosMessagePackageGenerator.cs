using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
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
        private readonly IRosPackageNameResolver _packageNameResolver;
        private readonly TypeNameMapper _typeNameMapper;

        public CodeGenerationPackageContext Package { get; }

        public RosMessagePackageGenerator(CodeGenerationPackageContext package, CodeGenerationOptions options,
            ProjectCodeGenerationDirectoryContext directories, IKeyedTemplateFormatter templateEngine)
        {
            Package = package ?? throw new ArgumentNullException(nameof(package));
            
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _directories = directories ?? throw new ArgumentNullException(nameof(directories));
            _templateEngine = templateEngine ?? throw new ArgumentNullException(nameof(templateEngine));

            _packageNameResolver =
                new UmlRosPackageNameResolver(new SingleKeyTemplateFormatter(TemplatePaths.PackageName,
                    templateEngine));

            _typeNameMapper = new TypeNameMapper(_packageNameResolver);
            
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
            _data.Package.Namespace = _packageNameResolver.ResolvePackageName(Package.PackageInfo.Name);
        }

        public void CreateProject()
        {
            CreateProjectFile();
            //TODO uncomment - it's to slow for fast debugging ;-)
            //AddNugetDependencies();
            
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

            _projectFilePath = projectFilePath;
        }

        private void AddNugetDependencies()
        {
            EnsurePackageData();
            
            var dependencies = new List<string>();
            dependencies.Add("Uml.Robotics.Ros.MessageBase");
            dependencies.AddRange(Package.Parser.PackageDependencies);
            
            foreach (var dependency in dependencies)
            {
                var packageName = _packageNameResolver.ResolvePackageName(dependency);

                var command = $"add \"{_projectFilePath}\" package {packageName} --no-restore";
                var process = RunDotNet(command);
            }
        }

        private void CreateMessages()
        {
            foreach (var message in Package.Parser.Messages)
            {
                CreateMessage(GetClassName(message.Key), message.Value);
            }
        }

        private static string GetClassName(string filename)
        {
            var ext = Path.GetExtension(filename);
            return filename
                .Substring(0, filename.Length - ext.Length)
                .Trim()
                .ToPascalCase();
        }

        private void CreateMessage(string name, MessageDescriptor message)
        {
            var hasHeaderField = message.Fields.Any(f => "std_msgs/Header".Equals(f.TypeInfo.ToString()));

            var fields = message.Fields.Select(x => new
            {
                RosType = x.TypeInfo,
                RosIdentifier = x.Identifier,
                Index = message.Items
                    .Select((item, index) => new { Item = item, Index = index})
                    .First(f => f.Item == x)
                    .Index + 1, // Index of this field in serialized message
                Type = new {
                    Name = _typeNameMapper.GetTypeName(x.TypeInfo),
                    IsPrimitive = x.TypeInfo.IsPrimitive,
                    IsArray = x.TypeInfo.IsArray,
                },
                Identifier = x.Identifier.ToPascalCase()
            });
            
            var data = new
            {
                Package = _data.Package,
                RosName = name,
                Name = name.ToPascalCase(),
                Fields = fields,
                HasHeaderField = hasHeaderField,
            };
            
            var fileName = $"{name}.cs";
            var content = _templateEngine.Format(TemplatePaths.MessageFile, data);

            WriteFile(fileName, content);
        }


        private void CreateServices()
        {
            foreach (var service in Package.Parser.Services)
            {
                CreateService(GetClassName(service.Key), service.Value);
            }
        }
        
        private void CreateService(string name, ServiceDescriptor service)
        {
            throw new NotImplementedException();
        }

        

        private void CreateActions()
        {
            foreach (var action in Package.Parser.Actions)
            {
                CreateAction(GetClassName(action.Key), action.Value);
            }
        }
        
        private void CreateAction(string name, ActionDescriptor action)
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