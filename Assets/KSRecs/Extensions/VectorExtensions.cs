using KSRecs.Utils;
using UnityEngine;

namespace KSRecs.Extensions
{
    public static class VectorExtensions
    {
        #region Vector3
        public static Vector3 Random(Vector3 min, Vector3 max) => new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
        public static float Distance(this Vector3 v1, Vector3 v2) => Vector3.Distance(v1, v2);
        public static float SquareDistance(this Vector3 v1, Vector3 v2) => Vector3.SqrMagnitude(v1 - v2);
        public static Vector3 Dot(this Vector3 v1, Vector3 v2) => new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        public static Vector3 InverseProduct(this Vector3 v1, Vector3 v2) => new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        public static Vector3 Rounded(this Vector3 value, Vector3 stepSize) => new Vector3(NumberUtils.Round(value.x, stepSize.x), NumberUtils.Round(value.y, stepSize.y), NumberUtils.Round(value.z, stepSize.z));
        public static int IndexOfClosest(this Vector3 vec, params Vector3[] vs)
        {
            int ind = 0;
            float minDis = float.MaxValue;
            float curDis;
            int count = 0;
            foreach (Vector3 v in vs)
            {
                curDis = SquareDistance(vec, v);
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
                curDis = SquareDistance(vec, v);
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
                curDis = SquareDistance(vec, v);
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
                curDis = SquareDistance(vec, v);
                if (curDis < maxDist)
                {
                    maxDist = count;
                }

                count++;
            }

            return maxDist;
        }
        #endregion

        #region Vector2
        public static Vector2 Random(Vector2 min, Vector2 max) => new Vector2(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y));
        public static float Distance(this Vector2 v1, Vector2 v2) => Vector2.Distance(v1, v2);
        public static float SquareDistance(this Vector2 v1, Vector2 v2) => Vector2.SqrMagnitude(v1 - v2);
        public static Vector2 Dot(this Vector2 v1, Vector2 v2) => new Vector2(v1.x * v2.x, v1.y * v2.y);
        public static Vector2 Divide(this Vector2 v1, Vector2 v2) => new Vector2(v1.x / v2.x, v1.y / v2.y);
        public static Vector2 Rounded(this Vector2 value, Vector2 stepSize) => new Vector2(NumberUtils.Round(value.x, stepSize.x), NumberUtils.Round(value.y, stepSize.y));
        public static int IndexOfClosest(this Vector2 vec, params Vector2[] vs)
        {
            int ind = 0;
            float minDis = float.MaxValue;
            float curDis;
            int count = 0;
            foreach (Vector2 v in vs)
            {
                curDis = SquareDistance(vec, v);
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
                curDis = SquareDistance(vec, v);
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
                curDis = SquareDistance(vec, v);
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
                curDis = SquareDistance(vec, v);
                if (curDis < maxDist)
                {
                    maxDist = count;
                }

                count++;
            }

            return maxDist;
        }
        #endregion
    }
}