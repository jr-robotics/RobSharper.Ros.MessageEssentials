using System;

namespace RobSharper.Ros.MessageEssentials
{
    public static class RosTimeExtensions
    {
        public static RosTime ToRosTime(this DateTime value)
        {
            return RosTime.FromDateTime(value);
        }

        public static RosTime ToRosTime(this TimeSpan value)
        {
            return RosTime.FromTimeSpan(value);
        }
    }
}