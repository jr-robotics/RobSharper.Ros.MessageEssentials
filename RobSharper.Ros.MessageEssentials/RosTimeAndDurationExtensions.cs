using System;

namespace RobSharper.Ros.MessageEssentials
{
    public static class RosTimeAndDurationExtensions
    {
        public static RosTime ToRosTime(this DateTime value)
        {
            return RosTime.FromDateTime(value);
        }

        public static RosDuration ToRosDuration(this TimeSpan value)
        {
            return RosDuration.FromTimeSpan(value);
        }
    }
}