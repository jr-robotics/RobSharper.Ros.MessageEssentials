using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobSharper.Ros.MessageBase
{
    public static class RosMessageTypeInfoExtensions
    {
        public static string GetFullTypeDefinition(this IRosMessageTypeInfo typeInfo)
        {
            if (typeInfo == null) throw new ArgumentNullException(nameof(typeInfo));
            
            var dependencies = new List<IRosMessageTypeInfo>();
            CollectDependencies(typeInfo, dependencies);
            
            var sb = new StringBuilder();

            sb.Append(typeInfo.MessageDefinition);

            foreach (var dependency in dependencies)
            {
                sb.Append("\n================================================================================\n");
                
                sb.Append("MSG: ");
                sb.Append(dependency.RosType);
                sb.Append("\n");
                sb.Append(dependency.MessageDefinition);
            }

            return sb.ToString();
        }

        private static void CollectDependencies(IRosMessageTypeInfo messageType, List<IRosMessageTypeInfo> dependencies)
        {
            foreach (var candidate in messageType.Dependencies)
            {
                var candidateType = candidate.RosType.ToString("T");
                
                if (dependencies.Any(x => x.RosType.ToString("T") == candidateType))
                    continue;

                dependencies.Add(candidate);
                CollectDependencies(candidate, dependencies);
            }
        }
    }
}