namespace RobSharper.Ros.MessageEssentials.Tests.RosMessages
{
    [RosMessage("geometry_msgs/Point")]
    public class Point
    {
        [RosMessageField("float64", "x", 1)]
        public double X { get; set; }
        
        [RosMessageField("float64", "y", 2)]
        public double Y { get; set; }
        
        [RosMessageField("float64", "z", 3)]
        public double Z { get; set; }
    }
}