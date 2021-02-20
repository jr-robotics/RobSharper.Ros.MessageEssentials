namespace RobSharper.Ros.MessageEssentials.Tests.RosMessages
{
    public static class EquivalentIntMessages
    {
        [RosMessage("equivalent_msgs/Int")]
        public class IntMessage
        {
            [RosMessageField("int16", "value", 1)]
            public int Value { get; set; }
            
            [RosMessageField("string", "text", 2)]
            public string Text { get; set; }
        }
        
        [RosMessage("equivalent_msgs/Short")]
        public class ShortMessage
        {
            [RosMessageField("int16", "value", 1)]
            public short Value { get; set; }
            
            [RosMessageField("string", "text", 2)]
            public string Text { get; set; }
        }
    }
}