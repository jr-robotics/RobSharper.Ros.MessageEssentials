using System;
using System.ComponentModel;
using System.Globalization;

namespace RobSharper.Ros.MessageEssentials
{
    public class RosDurationConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(TimeSpan) || 
                   sourceType == typeof(RosDuration);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(TimeSpan) || 
                   destinationType == typeof(RosDuration);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value != null)
            {
                if (value is RosDuration)
                    return (RosDuration) value;

                if (value is TimeSpan)
                    return ((TimeSpan) value).ToRosDuration();
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(TimeSpan))
                return ((RosDuration) value).TimeSpan;
            
            if (destinationType == typeof(RosDuration))
                return (RosDuration) value;
            
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}