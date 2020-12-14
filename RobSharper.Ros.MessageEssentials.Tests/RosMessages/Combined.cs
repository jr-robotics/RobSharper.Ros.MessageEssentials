namespace RobSharper.Ros.MessageEssentials.Tests.RosMessages
{
    [RosMessage("test_msgs/Combined")]
    public class Combined
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "6fdbcb03a86043f4030df29cabc53b0c";

        // Result of "gendeps --cat"
        public const string MESSAGE_DEFINITION = @"int32 CONST_A=13
int32 CONST_B=12
int32 fieldA
int32 fieldB";

        // Result of "gendeps --cat"
        public const string FULL_MESSAGE_DEFINITION = @"int32 CONST_A=13
int32 CONST_B=12
int32 fieldA
int32 fieldB";
        
        [RosMessageField("int32", "CONST_A", 2)]
        public const int CONST_A = 13;
        
        [RosMessageField("int32", "CONST_B", 4)]
        public const int CONST_B = 12;
        
        
        [RosMessageField("int32", "fieldA", 1)]
        public int A { get; set; }
        
        [RosMessageField("int32", "fieldB", 3)]
        public int B { get; set; }
    }
}