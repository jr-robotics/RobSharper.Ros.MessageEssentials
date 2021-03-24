using System;
using System.ComponentModel;

namespace RobSharper.Ros.MessageEssentials
{
    [TypeConverter(typeof(RosTimeConverter))]
    public struct RosTime : IConvertible
    {
        public static readonly DateTime EpochStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

        public static readonly RosTime Zero = new RosTime(0, 0);
        
        public int Seconds { get; }
        public int Nanoseconds { get; }

        private DateTime? _dateTime;
        public DateTime DateTime
        {
            get
            {
                if (!_dateTime.HasValue)
                {
                    _dateTime = EpochStart
                        .AddSeconds(Seconds)
                        .AddMilliseconds(Nanoseconds / 1000000.0);
                }

                return _dateTime.Value;
            }
        }

        public RosTime(int seconds = 0, int nanoseconds = 0)
        {
            _dateTime = null;
            
            Seconds = seconds;
            Nanoseconds = nanoseconds;
        }
        
        public static implicit operator RosTime(DateTime value) => FromDateTime(value);
        public static implicit operator DateTime(RosTime value) => value.DateTime;
        

        /// <summary>
        /// Converts a <see cref="System.DateTime"/> to a RosTime.
        /// 
        /// If DateTimeKind is Local, time is converted to UTC.
        /// It DateTimeKind is Unspecified, time is assumed to be UTC.
        /// If DateTimeKind is UTC, it is UTC.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static RosTime FromDateTime(DateTime value)
        {
            DateTime utcValue;

            switch (value.Kind)
            {
                case DateTimeKind.Utc:
                    utcValue = value;
                    break;
                case DateTimeKind.Local:
                    utcValue = value.ToUniversalTime();
                    break;
                case DateTimeKind.Unspecified:
                    utcValue = DateTime.SpecifyKind(value, DateTimeKind.Utc);
                    break;
                default:
                    throw new NotSupportedException($"DateTimeKind {value.Kind} is not supported");
            }

            var unixTimeSpan = (utcValue - EpochStart);

            var seconds = unixTimeSpan.Ticks / 10000000L;
            var nanoseconds = (unixTimeSpan.Ticks - (seconds * 10000000L)) * 100;

            return new RosTime((int)seconds, (int)nanoseconds);
        }
        

        TypeCode IConvertible.GetTypeCode()
        {
            return ((IConvertible) DateTime).GetTypeCode();
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return ((IConvertible) DateTime).ToBoolean(provider);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return ((IConvertible) DateTime).ToByte(provider);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return ((IConvertible) DateTime).ToChar(provider);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return DateTime;
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return ((IConvertible) DateTime).ToDecimal(provider);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return ((IConvertible) DateTime).ToDouble(provider);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return ((IConvertible) DateTime).ToInt16(provider);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return ((IConvertible) DateTime).ToInt32(provider);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return ((IConvertible) DateTime).ToInt64(provider);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return ((IConvertible) DateTime).ToSByte(provider);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return ((IConvertible) DateTime).ToSingle(provider);
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return ((IConvertible) DateTime).ToString(provider);
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return ((IConvertible) DateTime).ToType(conversionType, provider);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return ((IConvertible) DateTime).ToUInt16(provider);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return ((IConvertible) DateTime).ToUInt32(provider);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return ((IConvertible) DateTime).ToUInt64(provider);
        }
    }
}