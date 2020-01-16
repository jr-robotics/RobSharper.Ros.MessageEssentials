using System;
using System.Collections.Generic;
using Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.TemplateEngines;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration.UmlRobotics
{
    public class UmlRosPackageNameResolver : IRosPackageNameResolver
    {
        /// <summary>
        /// These packages are defined in Uml:Robotics.Ros.MessageBase
        /// </summary>
        public static IDictionary<string, string> SpecialPackages { get; } = new Dictionary<string, string>()
                  {
                      {"rosgraph_msgs", "Messages.rosgraph_msgs"}, 
                      {"std_msgs", "Messages.std_msgs"},
                      {"actionlib_msgs", "Messages.actionlib_msgs"},
                      {"geometry_msgs", "Messages.geometry_msgs"},
                      {"tf", "Messages.tf"},
                  };

        private readonly ITemplateFormatter _packageNameFormatter;

        public UmlRosPackageNameResolver(ITemplateFormatter packageNameTemplate)
        {
            _packageNameFormatter = packageNameTemplate ?? throw new ArgumentNullException(nameof(packageNameTemplate));
        }
        
        public string ResolvePackageName(string rosPackageName)
        {
            if (SpecialPackages.TryGetValue(rosPackageName, out var mappedPackageName))
            {
                return mappedPackageName;
            }
            
            var data = new
            {
                Name = rosPackageName,
                PascalName = rosPackageName.ToPascalCase()
            };
            
            return _packageNameFormatter
                .Format(data)
                .Trim();
        }
    }
}