using System;
using KSRecs.Utils;

namespace KSRecs.Extensions
{
    public static class DateTimeExtensions
    {
        public static int MinutesSinceDayStart(this DateTime dt) => DateTimeUtils.MinutesSinceDayStart(dt);  
        public static int SecondsSinceDayStart(this DateTime dt) => DateTimeUtils.SecondsSinceDayStart(dt);  
        public static int MillisecondsSinceDayStart(this DateTime dt) => DateTimeUtils.MillisecondsSinceDayStart(dt); 
        public static float SecondsSinceDayStart(this DateTime dt, bool includeMS) => DateTimeUtils.SecondsSinceDayStart(dt, includeMS); 
        public static float MinutesSinceDayStart(this DateTime dt, bool includeSecond, bool includeMS) => DateTimeUtils.MinutesSinceDayStart(dt, includeSecond, includeMS); 
        
        public static int CountMillisecondsTo(this DateTime from, DateTime to) => DateTimeUtils.CountMilliseconds(from, to);
        public static int CountSecondsTo(this DateTime from, DateTime to) => DateTimeUtils.CountSeconds(from, to);
        public static int CountMinutesTo(this DateTime from, DateTime to) => DateTimeUtils.CountMinutes(from, to);
        public static int CountHoursTo(this DateTime from, DateTime to) => DateTimeUtils.CountHours(from, to);
        public static int CountDaysTo(this DateTime from, DateTime to) => DateTimeUtils.CountDays(from, to);
        
        public static int CountMillisecondsFrom(this DateTime to, DateTime from) => DateTimeUtils.CountMilliseconds(from, to);
        public static int CountSecondsFrom(this DateTime to, DateTime from) => DateTimeUtils.CountSeconds(from, to);
        public static int CountMinutesFrom(this DateTime to, DateTime from) => DateTimeUtils.CountMinutes(from, to);
        public static int CountHoursFrom(this DateTime to, DateTime from) => DateTimeUtils.CountHours(from, to);
        public static int CountDaysFrom(this DateTime to, DateTime from) => DateTimeUtils.CountDays(from, to);
    }
}