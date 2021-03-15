namespace RobSharper.Ros.MessageEssentials.Tests.RosMessages
{
    [RosServiceMessage("std_srvs/SetBool", ServiceMessageKind.Request)]
    public class SetBoolRequest
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "8b94c1b53db61fb6aed406028ad6332a";

        // Result of "gendeps --cat"
        public const string MESSAGE_DEFINITION = @"bool data";
        
        
        [RosMessageField("bool", "data", 1)]
        public System.Boolean Data { get; set; }

    }
    
    [RosServiceMessage("std_srvs/SetBool", ServiceMessageKind.Response)]
    public class SetBoolResponse
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "937c9679a518e3a18d831e57125ea522";

        // Result of "gendeps --cat"
        public const string MESSAGE_DEFINITION = @"bool success
string message";
        
        
        [RosMessageField("bool", "success", 1)]
        public System.Boolean Success { get; set; }

        [RosMessageField("string", "message", 3)]
        public System.String Message { get; set; }

    }
}