namespace RobSharper.Ros.MessageEssentials.Tests.RosMessages
{
    [RosActionMessage("control_msgs/SingleJointPosition", ActionMessageKind.Goal)]
    public class SingleJointPositionGoal
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "fbaaa562a23a013fd5053e5f72cbb35c";

        // Result of "gendeps --cat"
        public const string MESSAGE_DEFINITION = @"float64 position
duration min_duration
float64 max_velocity";


        [RosMessageField("float64", "position", 1)]
        public System.Double Position { get; set; }
    
        [RosMessageField("duration", "min_duration", 2)]
        public System.TimeSpan MinDuration { get; set; }
    
        [RosMessageField("float64", "max_velocity", 3)]
        public System.Double MaxVelocity { get; set; }
    
    }
    
    [RosActionMessage("control_msgs/SingleJointPosition", ActionMessageKind.Result)]
    public class SingleJointPositionResult
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "d41d8cd98f00b204e9800998ecf8427e";

        // Result of "gendeps --cat"
        public const string MESSAGE_DEFINITION = @"";


    }
    
    [RosActionMessage("control_msgs/SingleJointPosition", ActionMessageKind.Feedback)]
    public class SingleJointPositionFeedback
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "8cee65610a3d08e0a1bded82f146f1fd";

        // Result of "gendeps --cat"
        public const string MESSAGE_DEFINITION = @"std_msgs/Header header
float64 position
float64 velocity
float64 error";
        
        
        [RosMessageField("std_msgs/Header", "header", 1)]
        public Header Header { get; set; } = new Header();
    
        [RosMessageField("float64", "position", 2)]
        public System.Double Position { get; set; }
    
        [RosMessageField("float64", "velocity", 3)]
        public System.Double Velocity { get; set; }
    
        [RosMessageField("float64", "error", 4)]
        public System.Double Error { get; set; }
    
    }
    
}