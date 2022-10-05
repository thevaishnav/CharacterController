using KSRecs.Utils;

namespace KSRecs.NumberExtensions
{
    public static class NumberExtensions
    {
        public static float ConvertRange(this float value, float oldMin, float oldMax, float newMin, float newMax) => NumberUtils.ConvertRange(value, oldMin, oldMax, newMin, newMax);
        public static int ConvertRange(this int value, int oldMin, int oldMax, int newMin, int newMax) => NumberUtils.ConvertRange(value, oldMin, oldMax, newMin, newMax);
        public static long ConvertRange(this long value, long oldMin, long oldMax, long newMin, long newMax) => NumberUtils.ConvertRange(value, oldMin, oldMax, newMin, newMax);
        public static double ConvertRange(this double value, double oldMin, double oldMax, double newMin, double newMax) => NumberUtils.ConvertRange(value, oldMin, oldMax, newMin, newMax);
        public static float ReverseRange(this float value, float minimum, float maximum) => NumberUtils.ReverseRange(value, minimum, maximum);
        public static int ReverseRange(this int value, int minimum, int maximum) => NumberUtils.ReverseRange(value, minimum, maximum);
        public static long ReverseRange(this long value, long minimum, long maximum) => NumberUtils.ReverseRange(value, minimum, maximum);
        public static double ReverseRange(this double value, double minimum, double maximum) => NumberUtils.ReverseRange(value, minimum, maximum);
    }
}
