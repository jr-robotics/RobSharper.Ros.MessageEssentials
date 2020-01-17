using System;
using System.Collections.Generic;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class TypeDependencyCollector : DefaultRosMessageVisitorListener
    {
        private readonly Dictionary<string, Tuple<string, string>> _dependencies = new Dictionary<string, Tuple<string, string>>();
        
        public IDictionary<string, Tuple<string, string>>  Dependencies => _dependencies;
        
        public override void OnVisitRosType(RosTypeInfo typeInfo)
        {
            var key = typeInfo.ToString();
            if (typeInfo.HasPackage && !_dependencies.ContainsKey(key))
            {
                _dependencies.Add(key, new Tuple<string, string>(typeInfo.PackageName, typeInfo.TypeName));
            }
        }
    }
}