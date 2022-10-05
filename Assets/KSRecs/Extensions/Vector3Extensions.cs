using UnityEngine;
using KSRecs.Utils;

namespace KSRecs.Vector3Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 Random(Vector3 min, Vector3 max)
        {
            return new Vector3(
                UnityEngine.Random.Range(min.x, max.x),
                UnityEngine.Random.Range(min.y, max.y),
                UnityEngine.Random.Range(min.z, max.z)
            );
        }

        public static Vector3 Dot(this Vector3 v1, Vector3 v2) => new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);

        public static Vector3 InverseProduct(this Vector3 v1, Vector3 v2) => new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);

        public static float Distance(this Vector3 v1, Vector3 v2) => Vector3.Distance(v1, v2);

        public static float SquareDistance(this Vector3 v1, Vector3 v2) => Vector3.SqrMagnitude(v1 - v2);

        public static int IndexOfClosest(this Vector3 vec, params Vector3[] vs)
        {
            int ind = 0;
            float minDis = float.MaxValue;
            float curDis;
            int count = 0;
            foreach (Vector3 v in vs)
            {
                curDis = vec.SquareDistance(v);
                if (curDis < minDis)
                {
                    minDis = curDis;
                    ind = count;
                }

                count++;
            }

            return ind;
        }

        public static float MinimumDistance(this Vector3 vec, params Vector3[] vs)
        {
            float minDis = float.MaxValue;
            float curDis;
            int count = 0;
            foreach (Vector3 v in vs)
            {
                curDis = vec.SquareDistance(v);
                if (curDis < minDis)
                {
                    minDis = curDis;
                }

                count++;
            }

            return minDis;
        }

        public static int IndexOfFurthest(this Vector3 vec, params Vector3[] vs)
        {
            int ind = 0;
            float maxDis = float.MinValue;
            float curDis;
            int count = 0;
            foreach (Vector3 v in vs)
            {
                curDis = vec.SquareDistance(v);
                if (curDis > maxDis)
                {
                    maxDis = curDis;
                    ind = count;
                }

                count++;
            }

            return ind;
        }

        public static float MaximumDistance(this Vector3 vec, params Vector3[] vs)
        {
            float maxDist = float.MaxValue;
            float curDis;
            int count = 0;
            foreach (Vector3 v in vs)
            {
                curDis = vec.SquareDistance(v);
                if (curDis < maxDist)
                {
                    maxDist = count;
                }

                count++;
            }

            return maxDist;
        }

        public static Vector3 ConvertRange(this Vector3 value, Vector3 oldMin, Vector3 oldMax, Vector3 newMin, Vector3 newMax)
        {
            if (oldMin == oldMax) return value;
            return ((value - oldMin).Dot(newMax - newMin).InverseProduct(oldMax - oldMin)) + newMin;
        }
        public static Vector3 ReverseRange(this Vector3 value, Vector3 minimum, Vector3 maximum) => ConvertRange(value, minimum, maximum, maximum, minimum);

        public static Vector3 Rounded(this Vector3 value, Vector3 stepSize)
        {
            return new Vector3(FloatUtils.Rounded(value.x, stepSize.x), FloatUtils.Rounded(value.y, stepSize.y),FloatUtils.Rounded(value.z, stepSize.z));
        }
    }
}