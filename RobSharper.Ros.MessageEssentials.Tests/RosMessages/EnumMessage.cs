namespace RobSharper.Ros.MessageEssentials.Tests.RosMessages
{
    [RosMessage("test_msgs/EnumMessage")]
    public class EnumMessage
    {
        public enum EnumValues
        {
            A = 13,
            B = 12
        }
        
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
        public const EnumValues CONST_A = EnumValues.A;
        
        [RosMessageField("int32", "CONST_B", 4)]
        public const EnumValues CONST_B = EnumValues.B;
        
        
        [RosMessageField("int32", "fieldA", 1)]
        public EnumValues A { get; set; }
        
        [RosMessageField("int32", "fieldB", 3)]
        public EnumValues B { get; set; }
    }
}