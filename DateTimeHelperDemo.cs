using System;

class HelloWorld
{
    static void Main()
    {
        DateTimeHelper.Configure("South Africa Standard Time");
        var dateNowSA = DateTimeHelper.GetSouthAfricaTimeNowWithOffSet();
        Console.WriteLine($"Date{dateNowSA}");
    }

    public static class DateTimeHelper
    {
        private static TimeZoneInfo _timeZone = TimeZoneInfo.Utc;

        public static void Configure(string timeZoneId)
        {
            _timeZone = FindTimeZone(timeZoneId);
        }

        private static TimeZoneInfo FindTimeZone(string timeZoneId)
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            catch (TimeZoneNotFoundException) when (timeZoneId == "South Africa Standard Time")
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Africa/Johannesburg");
            }
            catch (InvalidTimeZoneException) when (timeZoneId == "South Africa Standard Time")
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Africa/Johannesburg");
            }
        }

        public static DateTimeOffset GetSouthAfricaTimeNowWithOffSet()
        {
            // Get current UTC time
            var utcNow = DateTime.UtcNow;

            // Convert to configured timezone
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, _timeZone);

            // Get the correct offset for that timezone
            var offset = _timeZone.GetUtcOffset(utcNow);

            return new DateTimeOffset(localTime, offset);
        }

        public static DateTimeOffset GetSouthAfricaTimewithOffSet(DateTime? date)
        {
            // Convert to configured timezone
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(date.Value, _timeZone);

            // Get the correct offset for that timezone
            var offset = _timeZone.GetUtcOffset(date.Value);

            return new DateTimeOffset(localTime, offset);
        }

        public static bool IsLastDayOfMonth(DateTimeOffset dateTimeOffset)
        {
            int daysInMonth = DateTime.DaysInMonth(dateTimeOffset.Year, dateTimeOffset.Month);
            return dateTimeOffset.Day == daysInMonth;
        }

        public static DateTimeOffset GetStartOfMonth(DateTimeOffset date)
        {
            return new DateTimeOffset(date.Year, date.Month, 1, 0, 0, 0, date.Offset);
        }
    }
}
