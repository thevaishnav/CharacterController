using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using Random = UnityEngine.Random;

namespace KSRecs.Utils
{
    public static class ArrayUtils
    {
        public static T2[] AdvancedCopy<T1, T2>(T1[] source, Func<T1, T2> getterFunc)
        {
            T2[] arr = new T2[source.Length];
            Counter.Start();
            foreach (T1 t1v in source)
            {
                arr[Counter.Current] = getterFunc.Invoke(t1v);
            }

            return arr;
        }

        public static void AdvancedCopyInPlace<T1, T2>(T1[] source, ref T2[] copyTo, Func<T1, T2> getterFunc)
        {
            int maxLen = copyTo.Length;

            Counter.Start();
            foreach (T1 t1v in source)
            {
                if (Counter.CurrentStay == maxLen) return;
                copyTo[Counter.Current] = getterFunc.Invoke(t1v);
            }
        }

        public static T RandomElement<T>(T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }

        public static int RandomIndex<T>(T[] array)
        {
            return Random.Range(0, array.Length);
        }

        public static void RandomizeInPlace<T>(T[] array)
        {
            int count = array.Length;
            int randomIndex;
            T element;

            for (int i = 0; i < array.Length; i++)
            {
                randomIndex = Random.Range(0, count);
                element = array[randomIndex];
                array[randomIndex] = array[i];
                array[i] = element;
            }
        }

        public static T[] Randomized<T>(T[] array)
        {
            T[] newArray = new T[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                newArray[i] = array[i];
            }

            RandomizeInPlace(newArray);
            return newArray;
        }

        public static int ToLoopingIndex<T>(T[] array, int index)
        {
            return index % array.Length;
        }

        public static T ElementAtLoopingIndex<T>(T[] array, int index)
        {
            return array[index % array.Length];
        }

        public static T[] PySubArray<T>(T[] array, int start, int end = -1, int step = 1)
        {
            int endNew = end;
            if (endNew < 0)
            {
                endNew = array.Length + 1 + endNew;
            }

            T[] newList = new T[(int)(endNew - start) / step];

            Counter.Start();
            for (int i = start; i < endNew; i += step)
            {
                newList[Counter.Current] = array[i];
            }

            return newList;
        }

        public static KeyValuePair<int, int> IndexOfClosestPair<T>(T[] array, Func<T, T, float> distanceFunction)
        {
            int minOut = 0;
            int minIn = 0;
            float minDis = float.MaxValue;
            int cnt = array.Length;

            for (int co = 0; co < cnt; co++)
            {
                for (int ci = co; ci < cnt; ci++)
                {
                    float curDis = distanceFunction.Invoke(array[co], array[ci]);
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

        public static KeyValuePair<int, int> IndexOfFurthestPair<T>(T[] array, Func<T, T, float> distanceFunction)
        {
            int maxOut = 0;
            int maxIn = 0;
            float maxDis = float.MinValue;
            int cnt = array.Length;

            for (int co = 0; co < cnt; co++)
            {
                for (int ci = co; ci < cnt; ci++)
                {
                    float curDis = distanceFunction.Invoke(array[co], array[ci]);
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

        public static float SmallestDistance<T>(T[] array, Func<T, T, float> distanceFunction)
        {
            float minDis = float.MaxValue;
            int cnt = array.Length;

            for (int co = 0; co < cnt; co++)
            {
                for (int ci = co; ci < cnt; ci++)
                {
                    float curDis = distanceFunction.Invoke(array[co], array[ci]);
                    if (curDis < minDis)
                    {
                        minDis = curDis;
                    }
                }
            }

            return minDis;
        }

        public static float LargestDistance<T>(T[] array, Func<T, T, float> distanceFunction)
        {
            float maxDis = float.MinValue;
            int cnt = array.Length;

            for (int co = 0; co < cnt; co++)
            {
                for (int ci = co; ci < cnt; ci++)
                {
                    float curDis = distanceFunction.Invoke(array[co], array[ci]);
                    if (curDis > maxDis)
                    {
                        maxDis = curDis;
                    }
                }
            }

            return maxDis;
        }

        public static int IndexOfClosest<T>(T[] array, Func<T, float> distanceFunction)
        {
            int ind = 0;
            float minDis = float.MaxValue;
            float curDis;
            int count = 0;
            foreach (T v in array)
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

        public static int IndexOfFurthest<T>(T[] array, Func<T, float> distanceFunction)
        {
            int ind = 0;
            float maxDist = float.MinValue;
            float curDis;
            int count = 0;
            foreach (T v in array)
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

        public static float MinimumDistance<T>(T[] array, Func<T, float> distanceFunction)
        {
            float minDis = float.MaxValue;
            foreach (T v in array)
            {
                var curDis = distanceFunction.Invoke(v);
                if (curDis < minDis)
                {
                    minDis = curDis;
                }
            }

            return minDis;
        }

        public static float MaximumDistance<T>(T[] array, Func<T, float> distanceFunction)
        {
            float maxDist = float.MinValue;
            foreach (T v in array)
            {
                var curDis = distanceFunction.Invoke(v);
                if (curDis > maxDist)
                {
                    maxDist = curDis;
                }
            }

            return maxDist;
        }

        public static string ToStringFull<T>(T[] list)
        {
            if (list == null) return "null";

            StringBuilder builder = new StringBuilder();
            builder.Append("[");
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

            builder.Append("]");
            return builder.ToString();
        }


        #region Array of float
        public static KeyValuePair<int, int> IndexOfClosestPair(float[] array) => IndexOfClosestPair(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(float[] array) => IndexOfFurthestPair(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float SmallestDistance(float[] array) => SmallestDistance(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float LargestDistance(float[] array) => LargestDistance(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static int IndexOfClosest(float[] array, float point) => IndexOfClosest(array, f => Mathf.Abs(point - f));
        public static int IndexOfFurthest(float[] array, float point) => IndexOfFurthest(array, f => Mathf.Abs(point - f));
        public static float MinimumDistance(float[] array, float point) => MinimumDistance(array, f => Mathf.Abs(point - f));
        public static float MaximumDistance(float[] array, float point) => MaximumDistance(array, f => Mathf.Abs(point - f));
        #endregion

        #region Array of int
        public static KeyValuePair<int, int> IndexOfClosestPair(int[] array) => IndexOfClosestPair(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(int[] array) => IndexOfFurthestPair(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float SmallestDistance(int[] array) => SmallestDistance(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float LargestDistance(int[] array) => LargestDistance(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static int IndexOfClosest(int[] array, int point) => IndexOfClosest(array, f => Mathf.Abs(point - f));
        public static int IndexOfFurthest(int[] array, int point) => IndexOfFurthest(array, f => Mathf.Abs(point - f));
        public static float MinimumDistance(int[] array, int point) => MinimumDistance(array, f => Mathf.Abs(point - f));
        public static float MaximumDistance(int[] array, int point) => MaximumDistance(array, f => Mathf.Abs(point - f));
        #endregion

        #region Array of double
        public static KeyValuePair<int, int> IndexOfClosestPair(double[] array) => IndexOfClosestPair(array, (f1, f2) => (float)Math.Abs(f1 - f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(double[] array) => IndexOfFurthestPair(array, (f1, f2) => (float)Math.Abs(f1 - f2));
        public static float SmallestDistance(double[] array) => SmallestDistance(array, (f1, f2) => (float)Math.Abs(f1 - f2));
        public static float LargestDistance(double[] array) => LargestDistance(array, (f1, f2) => (float)Math.Abs(f1 - f2));
        public static int IndexOfClosest(double[] array, double point) => IndexOfClosest(array, f => (float)Math.Abs(point - f));
        public static int IndexOfFurthest(double[] array, double point) => IndexOfFurthest(array, f => (float)Math.Abs(point - f));
        public static float MinimumDistance(double[] array, double point) => MinimumDistance(array, f => (float)Math.Abs(point - f));
        public static float MaximumDistance(double[] array, double point) => MaximumDistance(array, f => (float)Math.Abs(point - f));
        #endregion

        #region Array of long
        public static KeyValuePair<int, int> IndexOfClosestPair(long[] array) => IndexOfClosestPair(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(long[] array) => IndexOfFurthestPair(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float SmallestDistance(long[] array) => SmallestDistance(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float LargestDistance(long[] array) => LargestDistance(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static int IndexOfClosest(long[] array, long point) => IndexOfClosest(array, f => Mathf.Abs(point - f));
        public static int IndexOfFurthest(long[] array, long point) => IndexOfFurthest(array, f => Mathf.Abs(point - f));
        public static float MinimumDistance(long[] array, long point) => MinimumDistance(array, f => Mathf.Abs(point - f));
        public static float MaximumDistance(long[] array, long point) => MaximumDistance(array, f => Mathf.Abs(point - f));
        #endregion

        #region Array of Vector2
        public static KeyValuePair<int, int> IndexOfClosestPair(Vector2[] array) => IndexOfClosestPair(array, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static KeyValuePair<int, int> IndexOfFurthestPair(Vector2[] array) => IndexOfFurthestPair(array, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static float SmallestDistance(Vector2[] array) => SmallestDistance(array, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static float LargestDistance(Vector2[] array) => LargestDistance(array, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static int IndexOfClosest(Vector2[] array, Vector2 point) => IndexOfClosest(array, f => (f - point).sqrMagnitude);
        public static int IndexOfFurthest(Vector2[] array, Vector2 point) => IndexOfFurthest(array, f => (f - point).sqrMagnitude);
        public static float MinimumDistance(Vector2[] array, Vector2 point) => MinimumDistance(array, f => (f - point).sqrMagnitude);
        public static float MaximumDistance(Vector2[] array, Vector2 point) => MaximumDistance(array, f => (f - point).sqrMagnitude);
        #endregion

        #region Array of Vector2
        public static KeyValuePair<int, int> IndexOfClosestPair(Vector3[] array) => IndexOfClosestPair(array, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static KeyValuePair<int, int> IndexOfFurthestPair(Vector3[] array) => IndexOfFurthestPair(array, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static float SmallestDistance(Vector3[] array) => SmallestDistance(array, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static float LargestDistance(Vector3[] array) => LargestDistance(array, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static int IndexOfClosest(Vector3[] array, Vector3 point) => IndexOfClosest(array, f => (f - point).sqrMagnitude);
        public static int IndexOfFurthest(Vector3[] array, Vector3 point) => IndexOfFurthest(array, f => (f - point).sqrMagnitude);
        public static float MinimumDistance(Vector3[] array, Vector3 point) => MinimumDistance(array, f => (f - point).sqrMagnitude);
        public static float MaximumDistance(Vector3[] array, Vector3 point) => MaximumDistance(array, f => (f - point).sqrMagnitude);
        #endregion

        #region Array of Quaternion
        public static KeyValuePair<int, int> IndexOfClosestPair(Quaternion[] array) => IndexOfClosestPair(array, (f1, f2) => Quaternion.Angle(f1, f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(Quaternion[] array) => IndexOfFurthestPair(array, (f1, f2) => Quaternion.Angle(f1, f2));
        public static float SmallestDistance(Quaternion[] array) => SmallestDistance(array, (f1, f2) => Quaternion.Angle(f1, f2));
        public static float LargestDistance(Quaternion[] array) => LargestDistance(array, (f1, f2) => Quaternion.Angle(f1, f2));
        public static int IndexOfClosest(Quaternion[] array, Quaternion point) => IndexOfClosest(array, f => Quaternion.Angle(f, point));
        public static int IndexOfFurthest(Quaternion[] array, Quaternion point) => IndexOfFurthest(array, f => Quaternion.Angle(f, point));
        public static float MinimumDistance(Quaternion[] array, Quaternion point) => MinimumDistance(array, f => Quaternion.Angle(f, point));
        public static float MaximumDistance(Quaternion[] array, Quaternion point) => MaximumDistance(array, f => Quaternion.Angle(f, point));
        #endregion
    }
}