using System;
using System.Globalization;
using UnityEngine;

namespace KSRecs.Utils
{
    public static class NumberUtils
    {
        public static float ConvertRange(float value, float oldMin, float oldMax, float newMin, float newMax)
        {
            if (oldMin == oldMax) return oldMin;
            return newMin + ((value - oldMin) * (newMax - newMin) / (oldMax - oldMin));
        }

        public static int ConvertRange(int value, int oldMin, int oldMax, int newMin, int newMax)
        {
            if (oldMin == oldMax) return oldMin;
            return newMin + ((value - oldMin) * (newMax - newMin) / (oldMax - oldMin));
        }

        public static long ConvertRange(long value, long oldMin, long oldMax, long newMin, long newMax)
        {
            if (oldMin == oldMax) return oldMin;
            return newMin + ((value - oldMin) * (newMax - newMin) / (oldMax - oldMin));
        }

        public static double ConvertRange(double value, double oldMin, double oldMax, double newMin, double newMax)
        {
            if (oldMin == oldMax) return oldMin;
            return newMin + ((value - oldMin) * (newMax - newMin) / (oldMax - oldMin));
        }

        public static float ReverseRange(float value, float minimum, float maximum)
        {
            if (minimum == maximum) return minimum;
            return maximum + ((value - minimum) * (minimum - maximum) / (maximum - minimum));
        }

        public static int ReverseRange(int value, int minimum, int maximum)
        {
            if (minimum == maximum) return minimum;
            return maximum + ((value - minimum) * (minimum - maximum) / (maximum - minimum));
        }

        public static long ReverseRange(long value, long minimum, long maximum)
        {
            if (minimum == maximum) return minimum;
            return maximum + ((value - minimum) * (minimum - maximum) / (maximum - minimum));
        }

        public static double ReverseRange(double value, double minimum, double maximum)
        {
            if (minimum == maximum) return minimum;
            return maximum + ((value - minimum) * (minimum - maximum) / (maximum - minimum));
        }
    }
}