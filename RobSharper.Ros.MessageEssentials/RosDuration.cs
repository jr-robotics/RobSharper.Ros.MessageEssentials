using System;
using System.ComponentModel;

namespace RobSharper.Ros.MessageEssentials
{
    [TypeConverter(typeof(RosDurationConverter))]
    public struct RosDuration
    {
        public static readonly RosDuration Zero = new RosDuration(0, 0);
        
        public int Seconds { get; }
        public int Nanoseconds { get; }

        private TimeSpan? _duration;

        public TimeSpan TimeSpan
        {
            get
            {
                if (!_duration.HasValue)
                {
                    _duration = new TimeSpan(0, 0, 0, Seconds, Nanoseconds / 1000000);
                }

                return _duration.Value;
            }
        }

        public RosDuration(int seconds = 0, int nanoseconds = 0)
        {
            _duration = null;
            
            Seconds = seconds;
            Nanoseconds = nanoseconds;
        }
        
        public static implicit operator TimeSpan(RosDuration value) => value.TimeSpan;
        public static implicit operator RosDuration(TimeSpan value) => FromTimeSpan(value);

        public static RosDuration FromTimeSpan(TimeSpan timeSpan)
        {
            var seconds = (int) timeSpan.TotalSeconds;
            var nanoseconds = timeSpan.Milliseconds * 1000000;

            return new RosDuration(seconds, nanoseconds);
        }
    }
}