using System;
using System.ComponentModel;
using FluentAssertions;
using Xunit;

namespace RobSharper.Ros.MessageEssentials.Tests
{
    public class RosTimeTests
    {
        [Fact]
        public void Zero_has_zero_values()
        {
            var rt = RosTime.Zero;

            rt.Seconds.Should().Be(0);
            rt.Nanoseconds.Should().Be(0);
        }

        [Fact]
        public void Zero_equals_epoch_start()
        {
            var rt = RosTime.Zero;

            rt.DateTime.Should().Be(RosTime.EpochStart);
        }

        [Fact]
        public void Can_convert_between_DateTime_and_RosTime()
        {
            var dateTime1 = new DateTime(2021, 03, 15, 18, 55, 12, 7, DateTimeKind.Utc);
            var rosTime = dateTime1.ToRosTime();
            var dateTime2 = rosTime.DateTime;

            dateTime1.Should().Be(dateTime2);
        }

        [Fact]
        public void Two_RosTimes_with_the_same_values_are_equal()
        {
            var dateTime = new DateTime(2021, 03, 15, 18, 55, 12, 7, DateTimeKind.Utc);
            
            var rosTime1 = dateTime.ToRosTime();
            var rosTime2 = dateTime.ToRosTime();

            rosTime1.Should().Be(rosTime2);
        }

        [Fact]
        public void Can_cast_RosTime_to_DateTime()
        {
            var dateTime1 = new DateTime(2021, 03, 15, 18, 55, 12, 7, DateTimeKind.Utc);
            var rosTime = dateTime1.ToRosTime();
            var dateTime2 = (DateTime) rosTime;

            dateTime1.Should().Be(dateTime2);
        }

        [Fact]
        public void Can_cast_DateTime_to_RosTime()
        {
            var dateTime1 = new DateTime(2021, 03, 15, 18, 55, 12, 7, DateTimeKind.Utc);
            var rosTime = (RosTime) dateTime1;
            var dateTime2 = (DateTime) rosTime;

            dateTime1.Should().Be(dateTime2);
        }

        [Fact]
        public void Can_convert_RosTime_to_DateTime_with_Convert_ChangeType()
        {
            var dateTime1 = new DateTime(2021, 03, 15, 18, 55, 12, 7, DateTimeKind.Utc);
            var rosTime = dateTime1.ToRosTime();
            var dateTime2 = (DateTime) Convert.ChangeType(rosTime, typeof(DateTime));

            dateTime1.Should().Be(dateTime2);
        }

        [Fact]
        public void Can_convert_RosTime_to_DateTime()
        {
            var dateTime1 = new DateTime(2021, 03, 15, 18, 55, 12, 7, DateTimeKind.Utc);
            var rosTime = dateTime1.ToRosTime();
            
            var converter = TypeDescriptor.GetConverter(typeof(RosTime));
            var dateTime2 = (DateTime) converter.ConvertTo(rosTime, typeof(DateTime));

            dateTime1.Should().Be(dateTime2);
        }

        [Fact]
        public void Can_convert_DateTime_to_RosTime()
        {
            var dateTime1 = new DateTime(2021, 03, 15, 18, 55, 12, 7, DateTimeKind.Utc);
            var converter = TypeDescriptor.GetConverter(typeof(RosTime));
            
            var rosTime = (RosTime) converter.ConvertFrom(dateTime1);
            var dateTime2 = rosTime.DateTime;

            dateTime1.Should().Be(dateTime2);
        }
    }
}