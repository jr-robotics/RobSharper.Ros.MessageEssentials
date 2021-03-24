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
                   sourceType == typeof(RosTime);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(DateTime) || 
                   destinationType == typeof(RosTime);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value != null)
            {
                if (value is RosTime)
                    return (RosTime) value;

                if (value is DateTime)
                    return ((DateTime) value).ToRosTime();
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(RosTime))
                return (RosTime) value;

            if (destinationType == typeof(DateTime))
                return ((RosTime) value).DateTime;
            
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}