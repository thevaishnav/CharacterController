using System;
using UnityEngine;

namespace KSRecs.Utils
{
    public static class IntUtils
    {
        public static int ConvertRange(int value, int oldMin, int oldMax, int newMin, int newMax)
        {
            if (oldMin == oldMax) return value;
            return ((value - oldMin) * (newMax - newMin) / (oldMax - oldMin)) + newMin;
        }

        public static int ConvertRange(int value,
            int oldMin, int oldMid, int oldMax,
            int newMin, int newMid, int newMax)
        {
            if (value < oldMin) return ConvertRange(value, oldMin, oldMid, newMin, newMid);
            if (value < oldMid) return ConvertRange(value, oldMin, oldMid, newMin, newMid);
            if (value == oldMid) return newMid;
            return ConvertRange(value, oldMid, oldMax, newMid, newMax);
        }

        public static int ConvertMilestones(int value, int[] oldMilestones, int[] newMilestones)
        {
            if (oldMilestones.Length != newMilestones.Length)
            {
                throw new ArgumentException(
                    $"number of oldMilestones must be same as number of newMilestones ({oldMilestones.Length} != {newMilestones.Length})"
                );
            }

            int previous = oldMilestones[0];
            int current;
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

        public static int ConvertRange(int value,
            int oldMin, int oldMax,
            int newMin, int newMax,
            AnimationCurve curve)
        {
            return (int)curve.Evaluate((float) ConvertRange(value, oldMin, oldMax, newMin, newMax));
        }

        public static int ConvertRange(int value,
            int oldMin, int oldMid, int oldMax,
            int newMin, int newMid, int newMax,
            AnimationCurve curve
        )
        {
            return (int)curve.Evaluate((float) ConvertRange(value, oldMin, oldMid, oldMax, newMin, newMid, newMax));
        }

        public static int ConvertMilestones(int value, int[] oldMilestones, int[] newMilestones,
            AnimationCurve curve)
        {
            return (int)curve.Evaluate((float) ConvertMilestones(value, oldMilestones, newMilestones));
        }

        public static int ConvertMilestones(int value, int[] oldMilestones, int[] newMilestones,
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

            int previous = oldMilestones[0];
            int current;
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

        public static int Rounded(int value, int stepSize)
        {
            int modulo = value % stepSize;
            if (modulo == 0)
            {
                return value;
            }

            if (modulo > (stepSize / 2))
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