using System;
using UnityEngine;


namespace KSRecs.Utils
{
    public static class LongUtils
    {
        public static long ConvertRange(long value, long oldMin, long oldMax, long newMin, long newMax)
        {
            if (oldMin == oldMax) return value;
            return ((value - oldMin) * (newMax - newMin) / (oldMax - oldMin)) + newMin;
        }

        public static long ConvertRange(long value,
            long oldMin, long oldMid, long oldMax,
            long newMin, long newMid, long newMax)
        {
            if (value < oldMin) return ConvertRange(value, oldMin, oldMid, newMin, newMid);
            if (value < oldMid) return ConvertRange(value, oldMin, oldMid, newMin, newMid);
            if (value == oldMid) return newMid;
            return ConvertRange(value, oldMid, oldMax, newMid, newMax);
        }

        public static long ConvertMilestones(long value, long[] oldMilestones, long[] newMilestones)
        {
            if (oldMilestones.Length != newMilestones.Length)
            {
                throw new ArgumentException(
                    $"number of oldMilestones must be same as number of newMilestones ({oldMilestones.Length} != {newMilestones.Length})"
                );
            }

            long previous = oldMilestones[0];
            long current;
            for (int i = 1; i < oldMilestones.Length; i++)
            {
                current = oldMilestones[i];
                if (previous <= value && value < current)
                {
                    return ConvertRange(value, previous, current, newMilestones[i - 1], newMilestones[i]);
                }

                previous = current;
            }

            return value;
        }

        public static long ConvertRange(long value,
            long oldMin, long oldMax,
            long newMin, long newMax,
            AnimationCurve curve)
        {
            return (long) curve.Evaluate((float) ConvertRange(value, oldMin, oldMax, newMin, newMax));
        }

        public static long ConvertRange(long value,
            long oldMin, long oldMid, long oldMax,
            long newMin, long newMid, long newMax,
            AnimationCurve curve
        )
        {
            return (long) curve.Evaluate((float) ConvertRange(value, oldMin, oldMid, oldMax, newMin, newMid, newMax));
        }

        public static long ConvertMilestones(long value, long[] oldMilestones, long[] newMilestones,
            AnimationCurve curve)
        {
            return (long) curve.Evaluate((float) ConvertMilestones(value, oldMilestones, newMilestones));
        }

        public static long ConvertMilestones(long value, long[] oldMilestones, long[] newMilestones,
            AnimationCurve[] curves)
        {
            if (oldMilestones.Length != newMilestones.Length)
            {
                throw new ArgumentException(
                    $"number of oldMilestones must be same as number of newMilestones ({oldMilestones.Length} != {newMilestones.Length})"
                );
            }

            if (oldMilestones.Length != newMilestones.Length)
            {
                throw new ArgumentException(
                    $"number of oldMilestones must be same as number of curves ({oldMilestones.Length} != {curves.Length})"
                );
            }

            long previous = oldMilestones[0];
            long current;
            for (int i = 1; i < oldMilestones.Length; i++)
            {
                current = oldMilestones[i];
                if (previous <= value && value < current)
                {
                    return ConvertRange(value, previous, current, newMilestones[i - 1], newMilestones[i], curves[i]);
                }

                previous = current;
            }

            return value;
        }

        public static long Rounded(long value, long stepSize)
        {
            long modulo = value % stepSize;
            if (modulo == 0f)
            {
                return value;
            }

            if (modulo > (stepSize / 2f))
            {
                return value + stepSize - modulo;
            }
            else
            {
                return value - modulo;
            }
        }
    }
}