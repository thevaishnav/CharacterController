using System;
using UnityEngine;
using System.Collections.Generic;
using KSRecs.Utils;

namespace KSRecs.EnumerableExtensions
{
    public static class ArrayExtensions
    {
        public static T2[] AdvancedCopy<T1, T2>(this T1[] source, Func<T1, T2> getterFunc) => ArrayUtils.AdvancedCopy(source, getterFunc);
        public static void AdvancedCopyInPlace<T1, T2>(this T1[] source, ref T2[] copyTo, Func<T1, T2> getterFunc) => ArrayUtils.AdvancedCopyInPlace(source, ref copyTo, getterFunc);
        public static T RandomElement<T>(this T[] list) => ArrayUtils.RandomElement(list);
        public static int RandomIndex<T>(this T[] list) => ArrayUtils.RandomIndex(list);
        public static void RandomizeInPlace<T>(this T[] list) => ArrayUtils.RandomizeInPlace(list);
        public static T[] Randomized<T>(this T[] list) => ArrayUtils.Randomized(list);
        public static int ToLoopingIndex<T>(this T[] list, int index) => ArrayUtils.ToLoopingIndex(list, index);
        public static T ElementAtLoopingIndex<T>(this T[] list, int index) => ArrayUtils.ElementAtLoopingIndex(list, index);
        public static T[] PySubArray<T>(this T[] list, int start, int end = -1, int step = 1) => ArrayUtils.PySubArray(list, start, end, step);
        public static KeyValuePair<int, int> IndexOfClosestPair<T>(this T[] list, Func<T, T, float> distanceFunction) => ArrayUtils.IndexOfClosestPair(list, distanceFunction);
        public static KeyValuePair<int, int> IndexOfFurthestPair<T>(this T[] list, Func<T, T, float> distanceFunction) => ArrayUtils.IndexOfFurthestPair(list, distanceFunction);
        public static float SmallestDistance<T>(this T[] list, Func<T, T, float> distanceFunction) => ArrayUtils.SmallestDistance(list, distanceFunction);
        public static float LargestDistance<T>(this T[] list, Func<T, T, float> distanceFunction) => ArrayUtils.LargestDistance(list, distanceFunction);
        public static int IndexOfClosest<T>(this T[] list, Func<T, float> distanceFunction) => ArrayUtils.IndexOfClosest(list, distanceFunction);
        public static int IndexOfFurthest<T>(this T[] list, Func<T, float> distanceFunction) => ArrayUtils.IndexOfFurthest(list, distanceFunction);
        public static float MinimumDistance<T>(this T[] list, Func<T, float> distanceFunction) => ArrayUtils.MinimumDistance(list, distanceFunction);
        public static float MaximumDistance<T>(this T[] list, Func<T, float> distanceFunction) => ArrayUtils.MaximumDistance(list, distanceFunction);
        public static string ToStringFull<T>(this T[] list) => ArrayUtils.ToStringFull(list);


        #region Array of float
        public static KeyValuePair<int, int> IndexOfClosestPair(this float[] array) => IndexOfClosestPair(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(this float[] array) => IndexOfFurthestPair(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float SmallestDistance(this float[] array) => SmallestDistance(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float LargestDistance(this float[] array) => LargestDistance(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static int IndexOfClosest(this float[] array, float point) => IndexOfClosest(array, f => Mathf.Abs(point - f));
        public static int IndexOfFurthest(this float[] array, float point) => IndexOfFurthest(array, f => Mathf.Abs(point - f));
        public static float MinimumDistance(this float[] array, float point) => MinimumDistance(array, f => Mathf.Abs(point - f));
        public static float MaximumDistance(this float[] array, float point) => MaximumDistance(array, f => Mathf.Abs(point - f));
        #endregion

        #region Array of int
        public static KeyValuePair<int, int> IndexOfClosestPair(this int[] array) => IndexOfClosestPair(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(this int[] array) => IndexOfFurthestPair(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float SmallestDistance(this int[] array) => SmallestDistance(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float LargestDistance(this int[] array) => LargestDistance(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static int IndexOfClosest(this int[] array, int point) => IndexOfClosest(array, f => Mathf.Abs(point - f));
        public static int IndexOfFurthest(this int[] array, int point) => IndexOfFurthest(array, f => Mathf.Abs(point - f));
        public static float MinimumDistance(this int[] array, int point) => MinimumDistance(array, f => Mathf.Abs(point - f));
        public static float MaximumDistance(this int[] array, int point) => MaximumDistance(array, f => Mathf.Abs(point - f));
        #endregion

        #region Array of double
        public static KeyValuePair<int, int> IndexOfClosestPair(this double[] array) => IndexOfClosestPair(array, (f1, f2) => (float)Math.Abs(f1 - f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(this double[] array) => IndexOfFurthestPair(array, (f1, f2) => (float)Math.Abs(f1 - f2));
        public static float SmallestDistance(this double[] array) => SmallestDistance(array, (f1, f2) => (float)Math.Abs(f1 - f2));
        public static float LargestDistance(this double[] array) => LargestDistance(array, (f1, f2) => (float)Math.Abs(f1 - f2));
        public static int IndexOfClosest(this double[] array, double point) => IndexOfClosest(array, f => (float)Math.Abs(point - f));
        public static int IndexOfFurthest(this double[] array, double point) => IndexOfFurthest(array, f => (float)Math.Abs(point - f));
        public static float MinimumDistance(this double[] array, double point) => MinimumDistance(array, f => (float)Math.Abs(point - f));
        public static float MaximumDistance(this double[] array, double point) => MaximumDistance(array, f => (float)Math.Abs(point - f));
        #endregion

        #region Array of long
        public static KeyValuePair<int, int> IndexOfClosestPair(this long[] array) => IndexOfClosestPair(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(this long[] array) => IndexOfFurthestPair(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float SmallestDistance(this long[] array) => SmallestDistance(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float LargestDistance(this long[] array) => LargestDistance(array, (f1, f2) => Mathf.Abs(f1 - f2));
        public static int IndexOfClosest(this long[] array, long point) => IndexOfClosest(array, f => Mathf.Abs(point - f));
        public static int IndexOfFurthest(this long[] array, long point) => IndexOfFurthest(array, f => Mathf.Abs(point - f));
        public static float MinimumDistance(this long[] array, long point) => MinimumDistance(array, f => Mathf.Abs(point - f));
        public static float MaximumDistance(this long[] array, long point) => MaximumDistance(array, f => Mathf.Abs(point - f));
        #endregion

        #region Array of Vector2
        public static KeyValuePair<int, int> IndexOfClosestPair(this Vector2[] array) => IndexOfClosestPair(array, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static KeyValuePair<int, int> IndexOfFurthestPair(this Vector2[] array) => IndexOfFurthestPair(array, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static float SmallestDistance(this Vector2[] array) => SmallestDistance(array, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static float LargestDistance(this Vector2[] array) => LargestDistance(array, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static int IndexOfClosest(this Vector2[] array, Vector2 point) => IndexOfClosest(array, f => (f - point).sqrMagnitude);
        public static int IndexOfFurthest(this Vector2[] array, Vector2 point) => IndexOfFurthest(array, f => (f - point).sqrMagnitude);
        public static float MinimumDistance(this Vector2[] array, Vector2 point) => MinimumDistance(array, f => (f - point).sqrMagnitude);
        public static float MaximumDistance(this Vector2[] array, Vector2 point) => MaximumDistance(array, f => (f - point).sqrMagnitude);
        #endregion

        #region Array of Vector2
        public static KeyValuePair<int, int> IndexOfClosestPair(this Vector3[] array) => IndexOfClosestPair(array, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static KeyValuePair<int, int> IndexOfFurthestPair(this Vector3[] array) => IndexOfFurthestPair(array, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static float SmallestDistance(this Vector3[] array) => SmallestDistance(array, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static float LargestDistance(this Vector3[] array) => LargestDistance(array, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static int IndexOfClosest(this Vector3[] array, Vector3 point) => IndexOfClosest(array, f => (f - point).sqrMagnitude);
        public static int IndexOfFurthest(this Vector3[] array, Vector3 point) => IndexOfFurthest(array, f => (f - point).sqrMagnitude);
        public static float MinimumDistance(this Vector3[] array, Vector3 point) => MinimumDistance(array, f => (f - point).sqrMagnitude);
        public static float MaximumDistance(this Vector3[] array, Vector3 point) => MaximumDistance(array, f => (f - point).sqrMagnitude);
        #endregion

        #region Array of Quaternion
        public static KeyValuePair<int, int> IndexOfClosestPair(this Quaternion[] array) => IndexOfClosestPair(array, (f1, f2) => Quaternion.Angle(f1, f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(this Quaternion[] array) => IndexOfFurthestPair(array, (f1, f2) => Quaternion.Angle(f1, f2));
        public static float SmallestDistance(this Quaternion[] array) => SmallestDistance(array, (f1, f2) => Quaternion.Angle(f1, f2));
        public static float LargestDistance(this Quaternion[] array) => LargestDistance(array, (f1, f2) => Quaternion.Angle(f1, f2));
        public static int IndexOfClosest(this Quaternion[] array, Quaternion point) => IndexOfClosest(array, f => Quaternion.Angle(f, point));
        public static int IndexOfFurthest(this Quaternion[] array, Quaternion point) => IndexOfFurthest(array, f => Quaternion.Angle(f, point));
        public static float MinimumDistance(this Quaternion[] array, Quaternion point) => MinimumDistance(array, f => Quaternion.Angle(f, point));
        public static float MaximumDistance(this Quaternion[] array, Quaternion point) => MaximumDistance(array, f => Quaternion.Angle(f, point));
        #endregion
    }
}