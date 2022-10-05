using System;
using UnityEngine;

namespace KSRecs.Utils
{
    public static class FloatUtils
    {
        public static float ConvertRange(float value, float oldMin, float oldMax, float newMin, float newMax)
        {
            if (oldMin == oldMax) return value;
            return ((value - oldMin) * (newMax - newMin) / (oldMax - oldMin)) + newMin;
        }

        public static float ConvertRange(float value,
            float oldMin, float oldMid, float oldMax,
            float newMin, float newMid, float newMax)
        {
            if (value < oldMin) return ConvertRange(value, oldMin, oldMid, newMin, newMid);
            if (value < oldMid) return ConvertRange(value, oldMin, oldMid, newMin, newMid);
            if (value == oldMid) return newMid;
            return ConvertRange(value, oldMid, oldMax, newMid, newMax);
        }

        public static float ConvertMilestones(float value, float[] oldMilestones, float[] newMilestones)
        {
            if (oldMilestones.Length != newMilestones.Length)
            {
                throw new ArgumentException(
                    $"number of oldMilestones must be same as number of newMilestones ({oldMilestones.Length} != {newMilestones.Length})"
                );
            }

            float previous = oldMilestones[0];
            float current;
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

        public static float ConvertRange(float value,
            float oldMin, float oldMax,
            float newMin, float newMax,
            AnimationCurve curve)
        {
            return curve.Evaluate((float) ConvertRange(value, oldMin, oldMax, newMin, newMax));
        }

        public static float ConvertRange(float value,
            float oldMin, float oldMid, float oldMax,
            float newMin, float newMid, float newMax,
            AnimationCurve curve
        )
        {
            return curve.Evaluate((float) ConvertRange(value, oldMin, oldMid, oldMax, newMin, newMid, newMax));
        }

        public static float ConvertMilestones(float value, float[] oldMilestones, float[] newMilestones,
            AnimationCurve curve)
        {
            return curve.Evaluate((float) ConvertMilestones(value, oldMilestones, newMilestones));
        }

        public static float ConvertMilestones(float value, float[] oldMilestones, float[] newMilestones,
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

            float previous = oldMilestones[0];
            float current;
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

        public static float Rounded(float value, float stepSize)
        {
            float modulo = value % stepSize;
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