using System;

namespace RobSharper.Ros.MessageEssentials.Tests.RosMessages
{
    [RosMessage("std_msgs/Header")]
    public class Header
    {
        [RosMessageField("uint32", "seq", 6)]
        public System.UInt32 Seq { get; set; }

        [RosMessageField("time", "stamp", 11)] public System.DateTime Stamp { get; set; } = new DateTime(1970, 01, 01);
    
        [RosMessageField("string", "frame_id", 13)]
        public System.String FrameId { get; set; } = String.Empty;
    
    }
}