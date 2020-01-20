namespace Joanneum.Robotics.Ros.MessageBase.Tests.RosMessages
{
    [RosMessageType("geometry_msgs", "Point")]
    public class Point
    {
        [RosMessageFieldDescriptor(1, "float64", "x")]
        public double X { get; set; }
        
        [RosMessageFieldDescriptor(1, "float64", "y")]
        public double Y { get; set; }
        
        [RosMessageFieldDescriptor(1, "float64", "z")]
        public double Z { get; set; }
    }
}