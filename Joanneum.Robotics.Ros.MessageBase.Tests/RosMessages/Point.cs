namespace Joanneum.Robotics.Ros.MessageBase.Tests.RosMessages
{
    [RosMessageType("geometry_msgs", "Point")]
    public class Point
    {
        [RosMessageField(1, "float64", "x")]
        public double X { get; set; }
        
        [RosMessageField(1, "float64", "y")]
        public double Y { get; set; }
        
        [RosMessageField(1, "float64", "z")]
        public double Z { get; set; }
    }
}