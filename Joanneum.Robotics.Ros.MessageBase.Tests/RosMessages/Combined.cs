namespace Joanneum.Robotics.Ros.MessageBase.Tests.RosMessages
{
    [RosMessageType("test_msgs/Combined")]
    public class Combined
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "6fdbcb03a86043f4030df29cabc53b0c";

        // Result of "gendeps --cat"
        public const string MESSAGE_DEFINITION = @"int32 CONST_A=13
int32 CONST_B=12
int32 fieldA
int32 fieldB";
        
        [RosMessageField(2, "int32", "CONST_A")]
        public const int CONST_A = 13;
        
        [RosMessageField(4, "int32", "CONST_B")]
        public const int CONST_B = 12;
        
        
        [RosMessageField(1, "int32", "fieldA")]
        public int A { get; set; }
        
        [RosMessageField(3, "int32", "fieldB")]
        public int B { get; set; }
    }
}