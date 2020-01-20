using System.Collections.Generic;

namespace Joanneum.Robotics.Ros.MessageBase.Tests.RosMessages
{
    [RosMessageType("test_msgs", "SimpleInt")]
    public class SimpleInt
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "5c9fb1a886e81e3162a5c87bf55c072b";
        
        [RosMessageField(1, "int32", "a")]
        public int A { get; set; }
    }
    
    [RosMessageType("test_msgs", "SimpleInt2")]
    public class SimpleInt2
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "5c9fb1a886e81e3162a5c87bf55c072b";
        
        [RosMessageField(1, "int32", "a")]
        public int A { get; set; }
    }

    [RosMessageType("test_msgs", "SimpleIntArray")]
    public class SimpleIntArray
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "089e88fbddca98397c0f90b12d01c8e0";

        [RosMessageField(1, "int32", "a")]
        public IList<int> A { get; set; } = new List<int>();
    }
    
    [RosMessageType("test_msgs", "NestedSimpleInt")]
    public class NestedSimpleInt
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "175e714d79cfd44e31c6b462f722c1e5";

        [RosMessageField(1, "test_msgs/SimpleInt", "a")]
        public SimpleInt A { get; set; } = new SimpleInt();
    }

    [RosMessageType("test_msgs", "NestedSimpleIntArray")]
    public class NestedSimpleIntArray
    {
        // Result of "rosmsg md5"
        public const string ROS_MD5 = "175e714d79cfd44e31c6b462f722c1e5";

        [RosMessageField(1, "test_msgs/SimpleInt[]", "a")]
        public IList<SimpleInt> A { get; set; } = new List<SimpleInt>();
    }
}