using UnityEngine;
using KSRecs.Utils;


namespace KSRecs.Vector2Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 Random(Vector2 min, Vector2 max)
        {
            return new Vector2(
                UnityEngine.Random.Range(min.x, max.x),
                UnityEngine.Random.Range(min.y, max.y)
            );
        }
        
        public static Vector2 Dot(this Vector2 v1, Vector2 v2) => new Vector2(v1.x * v2.x, v1.y * v2.y);

        public static Vector2 InverseProduct(this Vector2 v1, Vector2 v2) => new Vector2(v1.x / v2.x, v1.y / v2.y);

        public static float Distance(this Vector2 v1, Vector2 v2) => Vector2.Distance(v1, v2);

        public static float SquareDistance(this Vector2 v1, Vector2 v2) => Vector2.SqrMagnitude(v1 - v2);

        public static int IndexOfClosest(this Vector2 vec, params Vector2[] vs)
        {
            int ind = 0;
            float minDis = float.MaxValue;
            float curDis;
            int count = 0;
            foreach (Vector2 v in vs)
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

        public static float MinimumDistance(this Vector2 vec, params Vector2[] vs)
        {
            float minDis = float.MaxValue;
            float curDis;
            int count = 0;
            foreach (Vector2 v in vs)
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

        public static int IndexOfFurthest(this Vector2 vec, params Vector2[] vs)
        {
            int ind = 0;
            float maxDis = float.MinValue;
            float curDis;
            int count = 0;
            foreach (Vector2 v in vs)
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

        public static float MaximumDistance(this Vector2 vec, params Vector2[] vs)
        {
            float maxDist = float.MaxValue;
            float curDis;
            int count = 0;
            foreach (Vector2 v in vs)
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

        public static Vector2 ConvertRange(this Vector2 value, Vector2 oldMin, Vector2 oldMax, Vector2 newMin, Vector2 newMax)
        {
            if (oldMin == oldMax) return value;
            return ((value - oldMin).Dot(newMax - newMin).InverseProduct(oldMax - oldMin)) + newMin;
        }
        public static Vector2 ReverseRange(this Vector2 value, Vector2 minimum, Vector2 maximum) => ConvertRange(value, minimum, maximum, maximum, minimum);

        public static Vector2 Rounded(this Vector2 value, Vector2 stepSize)
        {
            return new Vector2(FloatUtils.Rounded(value.x, stepSize.x), FloatUtils.Rounded(value.y, stepSize.y));
        }
    }
}