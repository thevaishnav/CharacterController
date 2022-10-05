using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KSRecs.Utils
{
    public static class ListUtils
    {
        public static List<T2> AdvancedCopy<T1, T2>(List<T1> source, Func<T1, T2> getterFunc)
        {
            List<T2> lis = new List<T2>();
            foreach (T1 t1v in source)
            {
                lis.Add(getterFunc.Invoke(t1v));
            }

            return lis;
        }

        public static void AdvancedCopyInPlace<T1, T2>(List<T1> source, ref List<T2> copyTo, Func<T1, T2> getterFunc)
        {
            copyTo.Clear();

            foreach (T1 t1v in source)
            {
                copyTo.Add(getterFunc.Invoke(t1v));
            }
        }

        public static T RandomElement<T>(List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static void RandomizeInPlace<T>(List<T> list)
        {
            int count = list.Count;
            int randomIndex;
            T element;

            for (int i = 0; i < list.Count; i++)
            {
                randomIndex = Random.Range(0, count);
                element = list[randomIndex];
                list[randomIndex] = list[i];
                list[i] = element;
            }
        }

        public static List<T> Randomized<T>(List<T> list)
        {
            List<T> newList = new List<T>();
            for (int i = 0; i < list.Count; i++)
            {
                newList.Add(list[i]);
            }

            RandomizeInPlace(newList);
            return newList;
        }

        public static int ToLoopingIndex<T>(List<T> list, int index)
        {
            return index % list.Count;
        }

        public static T ElementAtLoopingIndex<T>(List<T> list, int index)
        {
            return list[index % list.Count];
        }

        public static List<T> PySubList<T>(List<T> list, int start, int end = -1, int step = 1)
        {
            if (step == 0)
            {
                throw new ArgumentException("Step of 0 is not allowed.");
            }

            List<T> newList = new List<T>();
            int endNew = end;
            if (endNew < 0)
            {
                endNew = list.Count + 1 + endNew;
            }


            for (int i = start; i < endNew; i += step)
            {
                newList.Add(list[i]);
            }

            return newList;
        }

        public static KeyValuePair<int, int> IndexOfClosestPair<T>(List<T> list, Func<T, T, float> distanceFunction)
        {
            int minOut = 0;
            int minIn = 0;
            float minDis = float.MaxValue;
            int cnt = list.Count;

            for (int co = 0; co < cnt; co++)
            {
                for (int ci = co; ci < cnt; ci++)
                {
                    float curDis = distanceFunction.Invoke(list[co], list[ci]);
                    if (curDis < minDis)
                    {
                        minDis = curDis;
                        minOut = co;
                        minIn = ci;
                    }
                }
            }

            return new KeyValuePair<int, int>(minIn, minOut);
        }

        public static KeyValuePair<int, int> IndexOfFurthestPair<T>(List<T> list, Func<T, T, float> distanceFunction)
        {
            int maxOut = 0;
            int maxIn = 0;
            float maxDis = float.MinValue;
            int cnt = list.Count;

            for (int co = 0; co < cnt; co++)
            {
                for (int ci = co; ci < cnt; ci++)
                {
                    float curDis = distanceFunction.Invoke(list[co], list[ci]);
                    if (curDis > maxDis)
                    {
                        maxDis = curDis;
                        maxOut = co;
                        maxIn = ci;
                    }
                }
            }

            return new KeyValuePair<int, int>(maxIn, maxOut);
        }

        public static float SmallestDistance<T>(List<T> list, Func<T, T, float> distanceFunction)
        {
            float minDis = float.MaxValue;
            int cnt = list.Count;

            for (int co = 0; co < cnt; co++)
            {
                for (int ci = co; ci < cnt; ci++)
                {
                    float curDis = distanceFunction.Invoke(list[co], list[ci]);
                    if (curDis < minDis)
                    {
                        minDis = curDis;
                    }
                }
            }

            return minDis;
        }

        public static float LargestDistance<T>(List<T> list, Func<T, T, float> distanceFunction)
        {
            float maxDis = float.MinValue;
            int cnt = list.Count;

            for (int co = 0; co < cnt; co++)
            {
                for (int ci = co; ci < cnt; ci++)
                {
                    float curDis = distanceFunction.Invoke(list[co], list[ci]);
                    if (curDis > maxDis)
                    {
                        maxDis = curDis;
                    }
                }
            }

            return maxDis;
        }

        public static int IndexOfClosest<T>(List<T> vs, Func<T, float> distanceFunction)
        {
            int ind = 0;
            float minDis = float.MaxValue;
            float curDis;
            int count = 0;
            foreach (T v in vs)
            {
                curDis = distanceFunction.Invoke(v);
                if (curDis < minDis)
                {
                    minDis = curDis;
                    ind = count;
                }

                count++;
            }

            return ind;
        }

        public static int IndexOfFurthest<T>(List<T> vs, Func<T, float> distanceFunction)
        {
            int ind = 0;
            float maxDist = float.MinValue;
            float curDis;
            int count = 0;
            foreach (T v in vs)
            {
                curDis = distanceFunction.Invoke(v);
                if (curDis > maxDist)
                {
                    maxDist = curDis;
                    ind = count;
                }

                count++;
            }

            return ind;
        }

        public static float MinimumDistance<T>(List<T> vs, Func<T, float> distanceFunction)
        {
            float minDis = float.MaxValue;
            foreach (T v in vs)
            {
                var curDis = distanceFunction.Invoke(v);
                if (curDis < minDis)
                {
                    minDis = curDis;
                }
            }

            return minDis;
        }

        public static float MaximumDistance<T>(List<T> vs, Func<T, float> distanceFunction)
        {
            float maxDist = float.MinValue;
            foreach (T v in vs)
            {
                var curDis = distanceFunction.Invoke(v);
                if (curDis > maxDist)
                {
                    maxDist = curDis;
                }
            }

            return maxDist;
        }

        public static string ToStringFull<T>(List<T> list)
        {
            if (list == null) return "null";

            StringBuilder builder = new StringBuilder();
            builder.Append("List Content: \n");
            foreach (T t in list)
            {
                if (t == null)
                {
                    builder.Append($"null, ");
                }
                else
                {
                    builder.Append($"{t}, ");
                }
            }

            return builder.ToString();
        }


        #region List of float
        public static KeyValuePair<int, int> IndexOfClosestPair(List<float> list) => IndexOfClosestPair(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(List<float> list) => IndexOfFurthestPair(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float SmallestDistance(List<float> list) => SmallestDistance(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float LargestDistance(List<float> list) => LargestDistance(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static int IndexOfClosest(List<float> list, float point) => IndexOfClosest(list, f => Mathf.Abs(point - f));
        public static int IndexOfFurthest(List<float> list, float point) => IndexOfFurthest(list, f => Mathf.Abs(point - f));
        public static float MinimumDistance(List<float> list, float point) => MinimumDistance(list, f => Mathf.Abs(point - f));
        public static float MaximumDistance(List<float> list, float point) => MaximumDistance(list, f => Mathf.Abs(point - f));
        #endregion

        #region List of int
        public static KeyValuePair<int, int> IndexOfClosestPair(List<int> list) => IndexOfClosestPair(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(List<int> list) => IndexOfFurthestPair(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float SmallestDistance(List<int> list) => SmallestDistance(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float LargestDistance(List<int> list) => LargestDistance(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static int IndexOfClosest(List<int> list, int point) => IndexOfClosest(list, f => Mathf.Abs(point - f));
        public static int IndexOfFurthest(List<int> list, int point) => IndexOfFurthest(list, f => Mathf.Abs(point - f));
        public static float MinimumDistance(List<int> list, int point) => MinimumDistance(list, f => Mathf.Abs(point - f));
        public static float MaximumDistance(List<int> list, int point) => MaximumDistance(list, f => Mathf.Abs(point - f));
        #endregion

        #region List of double
        public static KeyValuePair<int, int> IndexOfClosestPair(List<double> list) => IndexOfClosestPair(list, (f1, f2) => (float)Math.Abs(f1 - f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(List<double> list) => IndexOfFurthestPair(list, (f1, f2) => (float)Math.Abs(f1 - f2));
        public static float SmallestDistance(List<double> list) => SmallestDistance(list, (f1, f2) => (float)Math.Abs(f1 - f2));
        public static float LargestDistance(List<double> list) => LargestDistance(list, (f1, f2) => (float)Math.Abs(f1 - f2));
        public static int IndexOfClosest(List<double> list, double point) => IndexOfClosest(list, f => (float)Math.Abs(point - f));
        public static int IndexOfFurthest(List<double> list, double point) => IndexOfFurthest(list, f => (float)Math.Abs(point - f));
        public static float MinimumDistance(List<double> list, double point) => MinimumDistance(list, f => (float)Math.Abs(point - f));
        public static float MaximumDistance(List<double> list, double point) => MaximumDistance(list, f => (float)Math.Abs(point - f));
        #endregion

        #region List of long
        public static KeyValuePair<int, int> IndexOfClosestPair(List<long> list) => IndexOfClosestPair(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(List<long> list) => IndexOfFurthestPair(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float SmallestDistance(List<long> list) => SmallestDistance(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float LargestDistance(List<long> list) => LargestDistance(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static int IndexOfClosest(List<long> list, long point) => IndexOfClosest(list, f => Mathf.Abs(point - f));
        public static int IndexOfFurthest(List<long> list, long point) => IndexOfFurthest(list, f => Mathf.Abs(point - f));
        public static float MinimumDistance(List<long> list, long point) => MinimumDistance(list, f => Mathf.Abs(point - f));
        public static float MaximumDistance(List<long> list, long point) => MaximumDistance(list, f => Mathf.Abs(point - f));
        #endregion

        #region List of Vector2
        public static KeyValuePair<int, int> IndexOfClosestPair(List<Vector2> list) => IndexOfClosestPair(list, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static KeyValuePair<int, int> IndexOfFurthestPair(List<Vector2> list) => IndexOfFurthestPair(list, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static float SmallestDistance(List<Vector2> list) => SmallestDistance(list, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static float LargestDistance(List<Vector2> list) => LargestDistance(list, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static int IndexOfClosest(List<Vector2> list, Vector2 point) => IndexOfClosest(list, f => (f - point).sqrMagnitude);
        public static int IndexOfFurthest(List<Vector2> list, Vector2 point) => IndexOfFurthest(list, f => (f - point).sqrMagnitude);
        public static float MinimumDistance(List<Vector2> list, Vector2 point) => MinimumDistance(list, f => (f - point).sqrMagnitude);
        public static float MaximumDistance(List<Vector2> list, Vector2 point) => MaximumDistance(list, f => (f - point).sqrMagnitude);
        #endregion

        #region List of Vector2
        public static KeyValuePair<int, int> IndexOfClosestPair(List<Vector3> list) => IndexOfClosestPair(list, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static KeyValuePair<int, int> IndexOfFurthestPair(List<Vector3> list) => IndexOfFurthestPair(list, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static float SmallestDistance(List<Vector3> list) => SmallestDistance(list, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static float LargestDistance(List<Vector3> list) => LargestDistance(list, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static int IndexOfClosest(List<Vector3> list, Vector3 point) => IndexOfClosest(list, f => (f - point).sqrMagnitude);
        public static int IndexOfFurthest(List<Vector3> list, Vector3 point) => IndexOfFurthest(list, f => (f - point).sqrMagnitude);
        public static float MinimumDistance(List<Vector3> list, Vector3 point) => MinimumDistance(list, f => (f - point).sqrMagnitude);
        public static float MaximumDistance(List<Vector3> list, Vector3 point) => MaximumDistance(list, f => (f - point).sqrMagnitude);
        #endregion

        #region List of Quaternion
        public static KeyValuePair<int, int> IndexOfClosestPair(List<Quaternion> list) => IndexOfClosestPair(list, (f1, f2) => Quaternion.Angle(f1, f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(List<Quaternion> list) => IndexOfFurthestPair(list, (f1, f2) => Quaternion.Angle(f1, f2));
        public static float SmallestDistance(List<Quaternion> list) => SmallestDistance(list, (f1, f2) => Quaternion.Angle(f1, f2));
        public static float LargestDistance(List<Quaternion> list) => LargestDistance(list, (f1, f2) => Quaternion.Angle(f1, f2));
        public static int IndexOfClosest(List<Quaternion> list, Quaternion point) => IndexOfClosest(list, f => Quaternion.Angle(f, point));
        public static int IndexOfFurthest(List<Quaternion> list, Quaternion point) => IndexOfFurthest(list, f => Quaternion.Angle(f, point));
        public static float MinimumDistance(List<Quaternion> list, Quaternion point) => MinimumDistance(list, f => Quaternion.Angle(f, point));
        public static float MaximumDistance(List<Quaternion> list, Quaternion point) => MaximumDistance(list, f => Quaternion.Angle(f, point));
        #endregion
    }
}