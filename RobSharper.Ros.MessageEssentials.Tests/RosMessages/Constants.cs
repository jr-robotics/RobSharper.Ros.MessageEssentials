namespace RobSharper.Ros.MessageEssentials.Tests.RosMessages
{
    [RosMessage("test_msgs/LongConstant")]
    public class LongConstant
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "fbc80954bb9edd54cf83158e900ba6b1";

        // Result of "rosmsg show"
        public const string MESSAGE_DEFINITION = "int64 MY_CONSTANT=99";

        // Result of "rosmsg show"
        public const string FULL_MESSAGE_DEFINITION = "int64 MY_CONSTANT=99";
        
        [RosMessageField("int64", "MY_CONSTANT", 1)]
        public const long MY_CONSTANT = 99;
    }

    [RosMessage("test_msgs/FloatConstant")]
    public class FloatConstant
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "363f84c92248216749354201dcfc145c";

        // Result of "rosmsg show"
        public const string MESSAGE_DEFINITION = "float32 MY_CONSTANT=13.77";

        // Result of "rosmsg show"
        public const string FULL_MESSAGE_DEFINITION = "float32 MY_CONSTANT=13.77";
        
        [RosMessageField("float32", "MY_CONSTANT", 1)]
        public const float MY_CONSTANT = 13.77f;
    }
    
    [RosMessage("test_msgs/BoolConstant")]
    public class BoolConstant
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "2583893a8c876ed8ff3e699c08a52fd3";

        // Result of "rosmsg show"
        public const string MESSAGE_DEFINITION = "bool MY_CONSTANT=True";

        // Result of "rosmsg show"
        public const string FULL_MESSAGE_DEFINITION = "bool MY_CONSTANT=True";
        
        [RosMessageField("bool", "MY_CONSTANT", 1)]
        public const bool MY_CONSTANT = true;
    }
    
    [RosMessage("test_msgs/StringConstant")]
    public class StringConstant
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "f82e2404390c08e81e6dfcacbabe4279";

        // Result of "rosmsg show"
        public const string MESSAGE_DEFINITION = "string MY_CONSTANT=lorem ipsum";

        // Result of "rosmsg show"
        public const string FULL_MESSAGE_DEFINITION = "string MY_CONSTANT=lorem ipsum";
        
        [RosMessageField("string", "MY_CONSTANT", 1)]
        public const string MY_CONSTANT = "lorem ipsum";
    }
}