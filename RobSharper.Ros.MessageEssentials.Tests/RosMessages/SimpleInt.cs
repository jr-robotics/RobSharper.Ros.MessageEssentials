using System.Collections.Generic;

namespace RobSharper.Ros.MessageEssentials.Tests.RosMessages
{
    [RosMessage("test_msgs/SimpleInt")]
    public class SimpleInt
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "5c9fb1a886e81e3162a5c87bf55c072b";

        // Result of "rosmsg show"
        public const string FULL_MESSAGE_DEFINITION = "int32 a";

        // Result of "rosmsg show"
        public const string MESSAGE_DEFINITION = "int32 a";
        
        [RosMessageField("int32", "a", 1)]
        public int A { get; set; }
    }
    
    [RosMessage("test_msgs/SimpleIntField")]
    public class SimpleIntField
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "5c9fb1a886e81e3162a5c87bf55c072b";

        // Result of "rosmsg show"
        public const string FULL_MESSAGE_DEFINITION = "int32 a";

        // Result of "rosmsg show"
        public const string MESSAGE_DEFINITION = "int32 a";

        [RosMessageField("int32", "a", 1)] public int A;
    }
    
    [RosMessage("test_msgs/SimpleInt2")]
    public class SimpleInt2
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "5c9fb1a886e81e3162a5c87bf55c072b";

        // Result of "gendeps --cat"
        public const string MESSAGE_DEFINITION = "int32 a";

        // Result of "gendeps --cat"
        public const string FULL_MESSAGE_DEFINITION = "int32 a";
        
        [RosMessageField("int32", "a", 1)]
        public int A { get; set; }
    }

    [RosMessage("test_msgs/SimpleIntArray")]
    public class SimpleIntArray
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "089e88fbddca98397c0f90b12d01c8e0";

        // Result of "gendeps --cat"
        public const string MESSAGE_DEFINITION = "int32[] a";

        // Result of "gendeps --cat"
        public const string FULL_MESSAGE_DEFINITION = "int32[] a";

        [RosMessageField("int32[]", "a", 1)]
        public IList<int> A { get; set; } = new List<int>();
    }
    
    [RosMessage("test_msgs/NestedSimpleInt")]
    public class NestedSimpleInt
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "175e714d79cfd44e31c6b462f722c1e5";

        public const string MESSAGE_DEFINITION = @"test_msgs/SimpleInt a";

        // DEVIATED Result of "gendeps --cat"
        // gendeps uses no package name vor intra package types and header type
        public const string FULL_MESSAGE_DEFINITION = @"test_msgs/SimpleInt a
================================================================================
MSG: test_msgs/SimpleInt
int32 a";

        [RosMessageField("test_msgs/SimpleInt", "a", 1)]
        public SimpleInt A { get; set; } = new SimpleInt();
    }
    
    [RosMessage("test_msgs/NestedSimpleIntField")]
    public class NestedSimpleIntField
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "175e714d79cfd44e31c6b462f722c1e5";

        public const string MESSAGE_DEFINITION = @"test_msgs/SimpleIntField a";

        // DEVIATED Result of "gendeps --cat"
        // gendeps uses no package name vor intra package types and header type
        public const string FULL_MESSAGE_DEFINITION = @"test_msgs/SimpleIntField a
================================================================================
MSG: test_msgs/SimpleIntField
int32 a";

        [RosMessageField("test_msgs/SimpleIntField", "a", 1)]
        public SimpleIntField A = new SimpleIntField();
    }

    [RosMessage("test_msgs/NestedSimpleIntArray")]
    public class NestedSimpleIntArray
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "175e714d79cfd44e31c6b462f722c1e5";

        public const string MESSAGE_DEFINITION = @"test_msgs/SimpleInt[] a";
        
        // DEVIATED Result of "gendeps --cat"
        // gendeps uses no package name vor intra package types and header type
        public const string FULL_MESSAGE_DEFINITION = @"test_msgs/SimpleInt[] a
================================================================================
MSG: test_msgs/SimpleInt
int32 a";

        [RosMessageField("test_msgs/SimpleInt[]", "a", 1)]
        public IList<SimpleInt> A { get; set; } = new List<SimpleInt>();
    }

    [RosMessage("test_msgs/NestedNestedType")]
    public class NestedNestedType
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "3f8549369bb554ed39ebd1f74ac7ef00";

        // Result of "gendeps --cat"
        public const string MESSAGE_DEFINITION = @"test_msgs/NestedSimpleInt nestedField";

        // Result of "gendeps --cat"
        public const string FULL_MESSAGE_DEFINITION = @"test_msgs/NestedSimpleInt nestedField
================================================================================
MSG: test_msgs/NestedSimpleInt
test_msgs/SimpleInt a
================================================================================
MSG: test_msgs/SimpleInt
int32 a";

        [RosMessageField("test_msgs/NestedSimpleInt", "nestedField", 1)]
        public NestedSimpleInt NestedField { get; set; } = new NestedSimpleInt();
    }
}