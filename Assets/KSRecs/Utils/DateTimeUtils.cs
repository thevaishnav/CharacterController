using System;
using UnityEngine;

namespace KSRecs.Utils
{
    public static class DateTimeUtils
    {
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static DateTime MilliTimeStampToDateTime(double timeStamp)
        {
            TimeSpan time = TimeSpan.FromMilliseconds(timeStamp);
            DateTime date = new DateTime(1970, 1, 1).AddTicks(time.Ticks);
            return date;
        }

        public static bool HasDateArrived(string dt1, string dt2)
        {
            try
            {
                DateTime dateToCheck = DateTime.Parse(dt1);
                DateTime currentDate = DateTime.Parse(dt2);

                return HasDateArrived(dateToCheck, currentDate);
            }
            catch
            {
            }

            return false;
        }

        public static bool HasDateArrived(string dt1, DateTime dt2)
        {
            try
            {
                DateTime dateToCheck = DateTime.Parse(dt1);
                return HasDateArrived(dateToCheck, dt2);
            }
            catch
            {
            }

            return false;
        }

        public static bool HasDateArrived(DateTime dt1, DateTime dt2)
        {
            int value = DateTime.Compare(dt1, dt2);
            bool hasDateArrived = (value < 0) ? true : false;

            return hasDateArrived;
        }

        public static string TimeLeft(string dt1, string dt2, out int totalSeconds)
        {
            DateTime dateToCheck = DateTime.Parse(dt1);
            DateTime currentDate = DateTime.Parse(dt2);

            TimeSpan timeSpan = dateToCheck - currentDate;

            int daysdiff = Mathf.Abs(timeSpan.Days);
            int hoursdiff = Mathf.Abs(timeSpan.Hours);
            int minutesdiff = Mathf.Abs(timeSpan.Minutes);
            int secondsdiff = Mathf.Abs(timeSpan.Seconds);

            totalSeconds = int.Parse(timeSpan.TotalSeconds.ToString());

            return GetFormattedDate(daysdiff, hoursdiff, minutesdiff, secondsdiff);
        }

        public static string GetFormattedDate(int day, int hrs, int min, int sec)
        {
            string formattedTimerTxt;
            if (day > 0)
            {
                string dayStr = (day > 1) ? "days" : "day";
                formattedTimerTxt = string.Format("{0} {1}, {2:00}:{3:00}:{4:00}", day, dayStr, hrs, min, sec);
            }
            else
            {
                formattedTimerTxt = string.Format("{0:00}:{1:00}:{2:00}", hrs, min, sec);
            }

            return formattedTimerTxt;
        }

        public static string AddSeconds(string dt1, double seconds)
        {
            DateTime dateToCheck = DateTime.Parse(dt1);
            dateToCheck = dateToCheck.AddSeconds(seconds);

            return dateToCheck.ToString();
        }

        public static int CountMilliseconds(DateTime from, DateTime to) => (int) (to - from).TotalMilliseconds;
        public static int CountSeconds(DateTime from, DateTime to) => (int) (to - from).TotalSeconds;
        public static int CountMinutes(DateTime from, DateTime to) => (int) (to - from).TotalMinutes;
        public static int CountHours(DateTime from, DateTime to) => (int) (to - from).TotalHours;
        public static int CountDays(DateTime from, DateTime to) => (int) (to - from).TotalDays;

        public static int MinutesSinceDayStart(DateTime dt) => dt.Hour * 60 + dt.Minute;
        public static int SecondsSinceDayStart(DateTime dt) => dt.Hour * 3600 + dt.Minute * 60 + dt.Second;

        public static int MillisecondsSinceDayStart(DateTime dt) =>
            dt.Hour * 3600000 + dt.Minute * 60000 + dt.Second + 1000 + dt.Millisecond;

        public static float SecondsSinceDayStart(DateTime dt, bool includeMS)
        {
            if (includeMS) return dt.Hour * 3600f + dt.Minute * 60f + dt.Second + dt.Millisecond * 0.001f;
            ;
            return dt.Hour * 3600f + dt.Minute * 60f + dt.Second;
        }

        public static float MinutesSinceDayStart(DateTime dt, bool includeSecond, bool includeMS)
        {
            if (includeSecond && includeMS)
                return dt.Hour * 60 + dt.Minute + dt.Second / 60f + dt.Millisecond * 0.001f / 60f;
            if (includeSecond) return dt.Hour * 60 + dt.Minute + dt.Second / 60f;
            if (includeMS) return dt.Hour * 60 + dt.Minute + dt.Millisecond * 0.001f / 60f;
            return dt.Hour * 60 + dt.Minute;
        }
    }
}