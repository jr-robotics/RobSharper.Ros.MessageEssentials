using System;
using System.ComponentModel;
using FluentAssertions;
using Xunit;

namespace RobSharper.Ros.MessageEssentials.Tests
{
    public class RosDurationTests
    {
        [Fact]
        public void Zero_has_zero_values()
        {
            var rt = RosDuration.Zero;

            rt.Seconds.Should().Be(0);
            rt.Nanoseconds.Should().Be(0);
        }

        [Fact]
        public void Can_convert_between_TimeSpan_and_RosDuration()
        {
            var timeSpan1 = new TimeSpan(0, 1, 12, 25, 408);
            var rosDuration = timeSpan1.ToRosDuration();
            var timeSpan2 = rosDuration.TimeSpan;

            timeSpan1.Should().Be(timeSpan2);
        }

        [Fact]
        public void Two_RosDurations_with_the_same_values_are_equal()
        {
            var timeSpan1 = new TimeSpan(0, 1, 12, 25, 408);
            
            var rosDuration1 = timeSpan1.ToRosDuration();
            var rosDuration2 = timeSpan1.ToRosDuration();

            rosDuration1.Should().Be(rosDuration2);
        }

        [Fact]
        public void Can_cast_RosDuration_to_TimeSpan()
        {
            var timeSpan1 = new TimeSpan(0, 1, 12, 25, 408);
            var rosDuration = timeSpan1.ToRosDuration();
            var timeSpan2 = (TimeSpan) rosDuration;

            timeSpan1.Should().Be(timeSpan2);
        }

        [Fact]
        public void Can_cast_TimeSpan_to_RosDuration()
        {
            var timeSpan1 = new TimeSpan(0, 1, 12, 25, 408);
            var rosDuration = (RosDuration) timeSpan1;
            var timeSpan2 = (TimeSpan) rosDuration;

            timeSpan1.Should().Be(timeSpan2);
        }

        [Fact]
        public void Can_convert_RosDuration_to_TimeSpan()
        {
            var timeSpan1 = new TimeSpan(0, 1, 12, 25, 408);
            var rosDuration = timeSpan1.ToRosDuration();
            
            var converter = TypeDescriptor.GetConverter(typeof(RosDuration));
            var timeSpan2 = (TimeSpan) converter.ConvertTo(rosDuration, typeof(TimeSpan));

            timeSpan1.Should().Be(timeSpan2);
        }

        [Fact]
        public void Can_convert_TimeSpan_to_RosDuration()
        {
            var timeSpan1 = new TimeSpan(0, 1, 12, 25, 408);
            var converter = TypeDescriptor.GetConverter(typeof(RosDuration));
            
            var rosDuration = (RosDuration) converter.ConvertFrom(timeSpan1);
            var timeSpan2 = rosDuration.TimeSpan;

            timeSpan1.Should().Be(timeSpan2);
        }
    }
}