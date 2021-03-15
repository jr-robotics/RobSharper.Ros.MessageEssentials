using System;
using System.ComponentModel;
using System.Globalization;

namespace RobSharper.Ros.MessageEssentials
{
    public class RosTimeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(DateTime) || 
                   sourceType == typeof(RosTime) || 
                   sourceType == typeof(TimeSpan);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(DateTime) || 
                   destinationType == typeof(RosTime) || 
                   destinationType == typeof(TimeSpan);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value != null)
            {
                if (value is RosTime)
                    return (RosTime) value;

                if (value is DateTime)
                    return ((DateTime) value).ToRosTime();

                if (value is TimeSpan)
                    return ((TimeSpan) value).ToRosTime();
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(RosTime))
                return (RosTime) value;

            if (destinationType == typeof(DateTime))
                return ((RosTime) value).DateTime;

            if (destinationType == typeof(TimeSpan))
                return ((RosTime) value).TimeSpan;
            
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}