using System;
using System.Collections.Generic;
using RobSharper.Ros.MessageCli.CodeGeneration.TemplateEngines;
using RobSharper.Ros.MessageParser;

namespace RobSharper.Ros.MessageCli.CodeGeneration.UmlRobotics
{
    public class UmlRoboticsNameMapper : NameMapper
    {
        public struct RosMessageTypeMapping
        {
            public string TypeName { get; set; }
            public string NugetPackageName { get; set; }
        }

        /// <summary>
        /// These type definitions are not following the naming convention
        /// </summary>
        public static IDictionary<string, RosMessageTypeMapping> SpecialRosTypeMappings { get; } =
            new Dictionary<string, RosMessageTypeMapping>()
            {
                { "std_msgs/Header", new RosMessageTypeMapping{TypeName = "UmlRoboticsMessages.std_msgs.Header", NugetPackageName = "Uml.Robotics.Ros.MessageBase"}},
                { "std_msgs/Duration", new RosMessageTypeMapping{TypeName = "UmlRoboticsMessages.std_msgs.Duration", NugetPackageName = "Uml.Robotics.Ros.MessageBase"}},
                { "std_msgs/String", new RosMessageTypeMapping{TypeName = "UmlRoboticsMessages.std_msgs.String", NugetPackageName = "Uml.Robotics.Ros.MessageBase"}},
                { "std_msgs/Time", new RosMessageTypeMapping{TypeName = "UmlRoboticsMessages.std_msgs.Time", NugetPackageName = "Uml.Robotics.Ros.MessageBase"}},
                
                { "tf/tfMessage", new RosMessageTypeMapping{TypeName = "UmlRoboticsMessages.tf.tfMessage", NugetPackageName = "Uml.Robotics.Ros.MessageBase"}},
                
                { "rosgraph_msgs/Log", new RosMessageTypeMapping{TypeName = "UmlRoboticsMessages.rosgraph_msgs.Log", NugetPackageName = "Uml.Robotics.Ros.MessageBase"}},
                { "rosgraph_msgs/Clock", new RosMessageTypeMapping{TypeName = "UmlRoboticsMessages.rosgraph_msgs.Clock", NugetPackageName = "Uml.Robotics.Ros.MessageBase"}},
                
                { "geometry_msgs/Quaternion", new RosMessageTypeMapping{TypeName = "UmlRoboticsMessages.geometry_msgs.Quaternion", NugetPackageName = "Uml.Robotics.Ros.MessageBase"}},
                { "geometry_msgs/Transform", new RosMessageTypeMapping{TypeName = "UmlRoboticsMessages.geometry_msgs.Transform", NugetPackageName = "Uml.Robotics.Ros.MessageBase"}},
                { "geometry_msgs/TransformStamped", new RosMessageTypeMapping{TypeName = "UmlRoboticsMessages.geometry_msgs.TransformStamped", NugetPackageName = "Uml.Robotics.Ros.MessageBase"}},
                { "geometry_msgs/Vector3", new RosMessageTypeMapping{TypeName = "UmlRoboticsMessages.geometry_msgs.Vector3", NugetPackageName = "Uml.Robotics.Ros.MessageBase"}},
                
                { "actionlib_msgs/GoalID", new RosMessageTypeMapping{TypeName = "UmlRoboticsMessages.actionlib_msgs.GoalID", NugetPackageName = "Uml.Robotics.Ros.MessageBase"}},
                { "actionlib_msgs/GoalStatus", new RosMessageTypeMapping{TypeName = "UmlRoboticsMessages.actionlib_msgs.GoalStatus", NugetPackageName = "Uml.Robotics.Ros.MessageBase"}},
                { "actionlib_msgs/GoalStatusArray", new RosMessageTypeMapping{TypeName = "UmlRoboticsMessages.actionlib_msgs.GoalStatusArray", NugetPackageName = "Uml.Robotics.Ros.MessageBase"}},
            };

        public UmlRoboticsNameMapper(string packageName, ITemplateFormatter packageNamingConvention) : base(packageName, packageNamingConvention)
        {
        }

        protected override string ResolveTypeName(string rosPackageName, string rosTypeName)
        {
            if (rosPackageName == null) throw new ArgumentNullException(nameof(rosPackageName));
            if (rosTypeName == null) throw new ArgumentNullException(nameof(rosTypeName));
            
            if (TryGetTypeMapping(rosPackageName, rosTypeName, out var mapping))
            {
                return mapping.TypeName;
            }
            
            return base.ResolveTypeName(rosPackageName, rosTypeName);
        }
        
        public override string ResolveNugetPackageName(RosTypeInfo rosType)
        {
            if (rosType == null) throw new ArgumentNullException(nameof(rosType));
            
            if (TryGetTypeMapping(rosType.PackageName, rosType.TypeName, out var mapping))
            {
                return mapping.NugetPackageName;
            }

            return base.ResolveNugetPackageName(rosType);
        }

        private static bool TryGetTypeMapping(string rosPackageName, string rosTypeName, out RosMessageTypeMapping mapping)
        {
            return SpecialRosTypeMappings.TryGetValue($"{rosPackageName}/{rosTypeName}", out mapping);
        }

        public override bool IsBuiltInType(RosTypeInfo rosType)
        {
            var builtIn = base.IsBuiltInType(rosType);

            if (builtIn)
                return true;

            // Uml.Robotics.ROS has more types already defined in its libraries (e.g. Header)
            return SpecialRosTypeMappings.ContainsKey(rosType.ToString());
        }
    }
}