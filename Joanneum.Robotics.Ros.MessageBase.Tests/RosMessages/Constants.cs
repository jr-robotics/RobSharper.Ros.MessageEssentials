namespace Joanneum.Robotics.Ros.MessageBase.Tests.RosMessages
{
    [RosMessageType("test_msgs/LongConstant")]
    public class LongConstant
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "fbc80954bb9edd54cf83158e900ba6b1";

        // Result of "rosmsg show"
        public const string MESSAGE_DEFINITION = "int64 MY_CONSTANT=99";
        
        [RosMessageField(1, "int64", "MY_CONSTANT")]
        public const long MY_CONSTANT = 99;
    }

    [RosMessageType("test_msgs/FloatConstant")]
    public class FloatConstant
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "363f84c92248216749354201dcfc145c";

        // Result of "rosmsg show"
        public const string MESSAGE_DEFINITION = "float32 MY_CONSTANT=13.77";
        
        [RosMessageField(1, "float32", "MY_CONSTANT")]
        public const float MY_CONSTANT = 13.77f;
    }
    
    [RosMessageType("test_msgs/BoolConstant")]
    public class BoolConstant
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "2583893a8c876ed8ff3e699c08a52fd3";

        // Result of "rosmsg show"
        public const string MESSAGE_DEFINITION = "bool MY_CONSTANT=True";
        
        [RosMessageField(1, "bool", "MY_CONSTANT")]
        public const bool MY_CONSTANT = true;
    }
    
    [RosMessageType("test_msgs/StringConstant")]
    public class StringConstant
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "f82e2404390c08e81e6dfcacbabe4279";

        // Result of "rosmsg show"
        public const string MESSAGE_DEFINITION = "string MY_CONSTANT=lorem ipsum";
        
        [RosMessageField(1, "string", "MY_CONSTANT")]
        public const string MY_CONSTANT = "lorem ipsum";
    }
}