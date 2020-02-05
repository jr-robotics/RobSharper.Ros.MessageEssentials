using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.TemplateEngines;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.UmlRobotics;
using RobSharper.Ros.MessageParser;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.MessagePackage
{
    public class RosMessagePackageGenerator : IRosPackageGenerator
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

        public void Execute()
        {
            CreateProjectFile();
            AddNugetDependencies();
            
            CreateMessages();
            
            // TODO create other message types
            //CreateServices();
            //CreateActions();

            BuildProject();
            CopyOutput();
        }

        private void BuildProject()
        {
            var proc = DotNetProcess.Build(_projectFilePath);

            if (proc.ExitCode != 0)
            {
                // TODO: throw correct exception
                throw new Exception($"Build process exited with code {proc.ExitCode}");
            }
        }

        private void CreateProjectFile()
        {
            EnsurePackageData();

            var projectFilePath = _directories.TempDirectory.GetFilePath($"{_data.Package.Namespace}.csproj");
            var projectFileContent = _templateEngine.Format(TemplatePaths.ProjectFile, _data.Package);
            WriteFile(projectFilePath, projectFileContent);

            // TODO: Add temp nuget directory to nuget config (_directories.NugetTempDirectory)
            var nugetConfigFilePath = _directories.TempDirectory.GetFilePath("nuget.config");
            var nugetConfigFile = _templateEngine.Format(TemplatePaths.NugetConfigFile, null);
            WriteFile(nugetConfigFilePath, nugetConfigFile);

            _projectFilePath = projectFilePath;
        }

        private void AddNugetDependencies()
        {
            EnsurePackageData();

            IList<string> messageNugetPackages;
            
            // If package is a meta package use dependencies
            // form package info (parsed package.xml)
            // else use real dependencies retrieved form
            // message files.
            if (Package.PackageInfo.IsMetaPackage)
            {
                messageNugetPackages = Package.Parser
                    .PackageDependencies
                    .Select(x => _nameMapper.ResolveNugetPackageName(x))
                    .Distinct()
                    .ToList();
            }
            else
            {
                messageNugetPackages = Package.Parser
                    .ExternalTypeDependencies
                    .Select(x => _nameMapper.ResolveNugetPackageName(x))
                    .Distinct()
                    .ToList();
            }
            
            foreach (var dependency in messageNugetPackages)
            {
                var command = $"add \"{_projectFilePath}\" package {dependency}";
                var proc = DotNetProcess.Execute(command);
                
                if (proc.ExitCode != 0)
                {
                    // TODO: throw correct exception
                    throw new Exception($"Could not add dependency {dependency}. Process exited with code {proc.ExitCode}");
                }
            }
        }

        private void CopyOutput()
        {
            var nupkgFileName = $"{_data.Package.Namespace}.{_data.Package.Version}.nupkg";
            var nupkgSourceFile = new FileInfo(Path.Combine(_directories.TempDirectory.FullName, "bin", "Release", nupkgFileName));
            
            // Copy nuget package to temp package source, so it can be consumed by other projects in the current build pipeline
            var nugetTempDestination = new FileInfo(Path.Combine(_directories.NugetTempDirectory.FullName, nupkgFileName));
            ReplaceFiles(nupkgSourceFile, nugetTempDestination);
            
            
            // Copy nuget package to output directory if requested
            if (_options.CreateNugetPackage)
            {
                var nupkgDestinationFile = new FileInfo(Path.Combine(_directories.OutputDirectory.FullName, nupkgFileName));
                
                ReplaceFiles(nupkgSourceFile, nupkgDestinationFile);
            }

            
            // Copy dll to output directory if requested
            if (_options.CreateDll)
            {
                var dllFileName = $"{_data.Package.Namespace}.dll";
                
                var dllSourceFile = new FileInfo(Path.Combine(_directories.TempDirectory.FullName, "bin", "Release", "netstandard2.0", dllFileName));
                var dllDestinationFile = new FileInfo(Path.Combine(_directories.OutputDirectory.FullName, dllFileName));
                
                ReplaceFiles(dllSourceFile, dllDestinationFile);
            }
        }

        private static void ReplaceFiles(FileInfo sourceFile, FileInfo destinationFile)
        {
            if (destinationFile.Exists)
            {
                destinationFile.Delete();
            }
            else
            {
                var destinationDir = destinationFile.Directory;
                if (destinationDir != null && !destinationDir.Exists)
                {
                    destinationDir.Create();
                }
            }

            if (!sourceFile.Exists)
            {
                throw new InvalidOperationException($"Source file does not exist: {sourceFile}");
            }
            sourceFile.CopyTo(destinationFile.FullName);
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
            
            var filePath = _directories.TempDirectory.GetFilePath($"{className}.cs");
            var content = _templateEngine.Format(TemplatePaths.MessageFile, data);

            WriteFile(filePath, content);
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
            if (!Path.IsPathFullyQualified(filePath))
                throw new ArgumentException("File path must be fully qualified", nameof(filePath));
            
            File.WriteAllText(filePath, content);
        }

        
    }
}