using System;
using System.Text;

namespace TimeLibrary
{
    public static class TimeHelper
    {
        public static string GenerateUrlOffset(TimeSpan utcOffset)
        {
            var sb = new StringBuilder();
            if (utcOffset.Ticks < 0)
                sb.Append("minus");
            sb.Append(Math.Abs(utcOffset.Hours).ToString("D2"));
            sb.Append(Math.Abs(utcOffset.Minutes).ToString("D2"));
            return sb.ToString();
        }

        public static TimeSpan GetTimeDifference()
        {
            return GetTimeDifference(25);
        }

        public static TimeSpan GetTimeDifference(int xmasDay)
        {
            var dayOfChristmas = xmasDay;
            var christmasDayForCurrentYear = new DateTime(DateTime.Now.Year, 12, dayOfChristmas);
            var christmasDay = christmasDayForCurrentYear < DateTime.Now
                                   ? new DateTime(DateTime.Now.Year + 1, 12, dayOfChristmas, 0, 0, 0)
                                   : christmasDayForCurrentYear;
            return christmasDay - DateTime.Now;
        }
    }
}
