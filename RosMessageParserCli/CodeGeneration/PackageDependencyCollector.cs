using System.Collections.Generic;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class PackageDependencyCollector : DefaultRosMessageVisitorListener
    {
        private readonly HashSet<string> _dependencies = new HashSet<string>();

        public IEnumerable<string> Dependencies => _dependencies;

        
        public override void OnVisitRosType(RosTypeInfo typeInfo)
        {
            if (typeInfo.HasPackage && !_dependencies.Contains(typeInfo.PackageName))
            {
                _dependencies.Add(typeInfo.PackageName);
            }
        }
    }
}