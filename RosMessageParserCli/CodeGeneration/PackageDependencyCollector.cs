using System.Collections.Generic;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class PackageDependencyCollector : DefaultRosMessageVisitorListener
    {
        private readonly HashSet<string> _packageDependencies = new HashSet<string>();

        public IEnumerable<string> PackageDependencies => _packageDependencies;

        public override void OnVisitRosType(RosTypeInfo typeInfo)
        {
            if (typeInfo.HasPackage && !_packageDependencies.Contains(typeInfo.PackageName))
            {
                _packageDependencies.Add(typeInfo.PackageName);
            }
        }
    }
}