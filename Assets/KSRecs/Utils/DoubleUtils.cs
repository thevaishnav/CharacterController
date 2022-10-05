using System;
using UnityEngine;

namespace KSRecs.Utils
{
    public static class DoubleUtils
    {
        public static double ConvertRange(double value, double oldMin, double oldMax, double newMin, double newMax)
        {
            if (oldMin == oldMax) return value;
            return ((value - oldMin) * (newMax - newMin) / (oldMax - oldMin)) + newMin;
        }

        public static double ConvertRange(double value,
            double oldMin, double oldMid, double oldMax,
            double newMin, double newMid, double newMax)
        {
            if (value < oldMin) return ConvertRange(value, oldMin, oldMid, newMin, newMid);
            if (value < oldMid) return ConvertRange(value, oldMin, oldMid, newMin, newMid);
            if (value == oldMid) return newMid;
            return ConvertRange(value, oldMid, oldMax, newMid, newMax);
        }

        public static double ConvertMilestones(double value, double[] oldMilestones, double[] newMilestones)
        {
            if (oldMilestones.Length != newMilestones.Length)
            {
                throw new ArgumentException(
                    $"number of oldMilestones must be same as number of newMilestones ({oldMilestones.Length} != {newMilestones.Length})"
                );
            }

            double previous = oldMilestones[0];
            double current;
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

        public static double ConvertRange(double value,
            double oldMin, double oldMax,
            double newMin, double newMax,
            AnimationCurve curve)
        {
            return curve.Evaluate((float) ConvertRange(value, oldMin, oldMax, newMin, newMax));
        }

        public static double ConvertRange(double value,
            double oldMin, double oldMid, double oldMax,
            double newMin, double newMid, double newMax,
            AnimationCurve curve
        )
        {
            return curve.Evaluate((float) ConvertRange(value, oldMin, oldMid, oldMax, newMin, newMid, newMax));
        }

        public static double ConvertMilestones(double value, double[] oldMilestones, double[] newMilestones,
            AnimationCurve curve)
        {
            return curve.Evaluate((float) ConvertMilestones(value, oldMilestones, newMilestones));
        }

        public static double ConvertMilestones(double value, double[] oldMilestones, double[] newMilestones,
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

            double previous = oldMilestones[0];
            double current;
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

        public static double Rounded(double value, double stepSize)
        {
            double modulo = value % stepSize;
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