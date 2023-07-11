using System;

namespace KSRecs.Utils
{
    public static class DateTimeUtils
    {
        
        /// <summary> Converts UnixTimeStamps to DateTime </summary>
        /// <param name="timeStamp">UnixTimeStamps</param>
        /// <param name="isMillisecond">if timestamp is in milliseconds</param>
        public static DateTime FromTimeStamp(double timeStamp, bool isMillisecond)
        {
            TimeSpan time;
            if (isMillisecond) time = TimeSpan.FromMilliseconds(timeStamp);
            else time = TimeSpan.FromSeconds(timeStamp);
            return new DateTime(1970, 1, 1).AddTicks(time.Ticks);
        }

        /// <returns>True if date dt2 comes after date dt1</returns>
        public static bool HasDateArrived(DateTime dt1, DateTime dt2) => DateTime.Compare(dt1, dt2) < 0;

        /// <returns>Differance (in Milliseconds) between two dates (can be negative)</returns>
        public static int CountMilliseconds(DateTime from, DateTime to) => (int)(to - from).TotalMilliseconds;

        /// <returns>Differance (in Seconds) between two dates (can be negative)</returns>
        public static int CountSeconds(DateTime from, DateTime to) => (int)(to - from).TotalSeconds;
        
        /// <returns>Differance (in Minutes) between two dates (can be negative)</returns>
        public static int CountMinutes(DateTime from, DateTime to) => (int)(to - from).TotalMinutes;
        
        /// <returns>Differance (in Hours) between two dates (can be negative)</returns>
        public static int CountHours(DateTime from, DateTime to) => (int)(to - from).TotalHours;
        
        /// <returns>Differance (in Days) between two dates (can be negative)</returns>
        public static int CountDays(DateTime from, DateTime to) => (int)(to - from).TotalDays;

        /// <returns> Differance (in Minutes) between given time and start of the day (0H 0M 0S 0ms is considered as start of day) (can be negative)</returns>
        public static int MinutesSinceDayStart(DateTime dt) => dt.Hour * 60 + dt.Minute;

        /// <returns> Differance (in Seconds) between given time and start of the day (0H 0M 0S 0ms is considered as start of day) (can be negative)</returns>
        public static int SecondsSinceDayStart(DateTime dt) => dt.Hour * 3600 + dt.Minute * 60 + dt.Second;
        
        /// <returns> Differance (in Milliseconds) between given time and start of the day (0H 0M 0S 0ms is considered as start of day) (can be negative)</returns>
        public static int MillisecondsSinceDayStart(DateTime dt) => dt.Hour * 3600000 + dt.Minute * 60000 + dt.Second + 1000 + dt.Millisecond;

        /// <returns> Differance (in Seconds) between given time and start of the day (0H 0M 0S 0ms is considered as start of day) (can be negative)</returns>
        public static float SecondsSinceDayStart(DateTime dt, bool includeMS)
        {
            if (includeMS) return dt.Hour * 3600f + dt.Minute * 60f + dt.Second + dt.Millisecond * 0.001f;
            return dt.Hour * 3600f + dt.Minute * 60f + dt.Second;
        }
        
        /// <param name="includeSecond">if true then fractional part contains differance in Seconds</param>
        /// <param name="includeMS">if true then fractional part contains differance in MilliSeconds</param>
        /// <returns> Differance (in Minutes) between given time and start of the day (0H 0M 0S 0ms is considered as start of day) (can be negative)</returns>
        public static float MinutesSinceDayStart(DateTime dt, bool includeSecond, bool includeMS)
        {
            float minutes = dt.Hour * 60 + dt.Minute;
            if (includeSecond) minutes += dt.Second / 60f;
            if (includeMS) minutes += dt.Millisecond * 0.001f / 60f;
            return minutes;
        }
    }
}