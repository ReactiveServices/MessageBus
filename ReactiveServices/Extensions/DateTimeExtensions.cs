using System;
using System.Globalization;

namespace ReactiveServices.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTime(this DateTime dateTime)
        {
            var universalTime = dateTime.ToUniversalTime();
            var unixStartTime = new DateTime(1970, 1, 1);
            var timeSinceStart = universalTime - unixStartTime;
            return (long)timeSinceStart.TotalMilliseconds;
        }

        public static DateTime ToDateTime(this string utcString)
        {
            return DateTime.Parse(utcString, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }
    }
}
