using System;
using System.Collections.Generic;
using UnityEngine;
using KSRecs.Utils;

namespace KSRecs.EnumerableExtensions
{
    public static class ListExtensions
    {
        public static List<T2> AdvancedCopy<T1, T2>(this List<T1> source, Func<T1, T2> getterFunc) => ListUtils.AdvancedCopy<T1, T2>(source, getterFunc);
        public static void AdvancedCopyInPlace<T1, T2>(this List<T1> source, ref List<T2> copyTo, Func<T1, T2> getterFunc) => ListUtils.AdvancedCopyInPlace<T1, T2>(source, ref copyTo, getterFunc);
        public static T RandomElement<T>(this List<T> list) => ListUtils.RandomElement(list);
        public static void RandomizeInPlace<T>(this List<T> list) => ListUtils.RandomizeInPlace(list);
        public static List<T> Randomized<T>(this List<T> list) => ListUtils.Randomized(list);
        public static int ToLoopingIndex<T>(this List<T> list, int index) => ListUtils.ToLoopingIndex(list, index);
        public static T ElementAtLoopingIndex<T>(this List<T> list, int index) => ListUtils.ElementAtLoopingIndex(list, index);
        public static List<T> PySubList<T>(this List<T> list, int start, int end = -1, int step = 1) => ListUtils.PySubList(list, start, end, step);
        public static KeyValuePair<int, int> IndexOfClosestPair<T>(this List<T> list, Func<T, T, float> distanceFunction) => ListUtils.IndexOfClosestPair(list, distanceFunction);
        public static KeyValuePair<int, int> IndexOfFurthestPair<T>(this List<T> list, Func<T, T, float> distanceFunction) => ListUtils.IndexOfFurthestPair(list, distanceFunction);
        public static float SmallestDistance<T>(this List<T> list, Func<T, T, float> distanceFunction) => ListUtils.SmallestDistance(list, distanceFunction);
        public static float LargestDistance<T>(this List<T> list, Func<T, T, float> distanceFunction) => ListUtils.LargestDistance(list, distanceFunction);
        public static int IndexOfClosest<T>(this List<T> list, Func<T, float> distanceFunction) => ListUtils.IndexOfClosest(list, distanceFunction);
        public static int IndexOfFurthest<T>(this List<T> list, Func<T, float> distanceFunction) => ListUtils.IndexOfFurthest(list, distanceFunction);
        public static float MinimumDistance<T>(this List<T> list, Func<T, float> distanceFunction) => ListUtils.MinimumDistance(list, distanceFunction);
        public static float MaximumDistance<T>(this List<T> list, Func<T, float> distanceFunction) => ListUtils.MaximumDistance(list, distanceFunction);
        public static string ToStringFull<T>(this List<T> list) => ListUtils.ToStringFull(list);


        #region List of float
        public static KeyValuePair<int, int> IndexOfClosestPair(this List<float> list) => IndexOfClosestPair(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(this List<float> list) => IndexOfFurthestPair(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float SmallestDistance(this List<float> list) => SmallestDistance(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float LargestDistance(this List<float> list) => LargestDistance(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static int IndexOfClosest(this List<float> list, float point) => IndexOfClosest(list, f => Mathf.Abs(point - f));
        public static int IndexOfFurthest(this List<float> list, float point) => IndexOfFurthest(list, f => Mathf.Abs(point - f));
        public static float MinimumDistance(this List<float> list, float point) => MinimumDistance(list, f => Mathf.Abs(point - f));
        public static float MaximumDistance(this List<float> list, float point) => MaximumDistance(list, f => Mathf.Abs(point - f));
        #endregion

        #region List of int
        public static KeyValuePair<int, int> IndexOfClosestPair(this List<int> list) => IndexOfClosestPair(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(this List<int> list) => IndexOfFurthestPair(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float SmallestDistance(this List<int> list) => SmallestDistance(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float LargestDistance(this List<int> list) => LargestDistance(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static int IndexOfClosest(this List<int> list, int point) => IndexOfClosest(list, f => Mathf.Abs(point - f));
        public static int IndexOfFurthest(this List<int> list, int point) => IndexOfFurthest(list, f => Mathf.Abs(point - f));
        public static float MinimumDistance(this List<int> list, int point) => MinimumDistance(list, f => Mathf.Abs(point - f));
        public static float MaximumDistance(this List<int> list, int point) => MaximumDistance(list, f => Mathf.Abs(point - f));
        #endregion

        #region List of double
        public static KeyValuePair<int, int> IndexOfClosestPair(this List<double> list) => IndexOfClosestPair(list, (f1, f2) => (float)Math.Abs(f1 - f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(this List<double> list) => IndexOfFurthestPair(list, (f1, f2) => (float)Math.Abs(f1 - f2));
        public static float SmallestDistance(this List<double> list) => SmallestDistance(list, (f1, f2) => (float)Math.Abs(f1 - f2));
        public static float LargestDistance(this List<double> list) => LargestDistance(list, (f1, f2) => (float)Math.Abs(f1 - f2));
        public static int IndexOfClosest(this List<double> list, double point) => IndexOfClosest(list, f => (float)Math.Abs(point - f));
        public static int IndexOfFurthest(this List<double> list, double point) => IndexOfFurthest(list, f => (float)Math.Abs(point - f));
        public static float MinimumDistance(this List<double> list, double point) => MinimumDistance(list, f => (float)Math.Abs(point - f));
        public static float MaximumDistance(this List<double> list, double point) => MaximumDistance(list, f => (float)Math.Abs(point - f));
        #endregion

        #region List of long
        public static KeyValuePair<int, int> IndexOfClosestPair(this List<long> list) => IndexOfClosestPair(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(this List<long> list) => IndexOfFurthestPair(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float SmallestDistance(this List<long> list) => SmallestDistance(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static float LargestDistance(this List<long> list) => LargestDistance(list, (f1, f2) => Mathf.Abs(f1 - f2));
        public static int IndexOfClosest(this List<long> list, long point) => IndexOfClosest(list, f => Mathf.Abs(point - f));
        public static int IndexOfFurthest(this List<long> list, long point) => IndexOfFurthest(list, f => Mathf.Abs(point - f));
        public static float MinimumDistance(this List<long> list, long point) => MinimumDistance(list, f => Mathf.Abs(point - f));
        public static float MaximumDistance(this List<long> list, long point) => MaximumDistance(list, f => Mathf.Abs(point - f));
        #endregion

        #region List of Vector2
        public static KeyValuePair<int, int> IndexOfClosestPair(this List<Vector2> list) => IndexOfClosestPair(list, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static KeyValuePair<int, int> IndexOfFurthestPair(this List<Vector2> list) => IndexOfFurthestPair(list, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static float SmallestDistance(this List<Vector2> list) => SmallestDistance(list, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static float LargestDistance(this List<Vector2> list) => LargestDistance(list, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static int IndexOfClosest(this List<Vector2> list, Vector2 point) => IndexOfClosest(list, f => (f - point).sqrMagnitude);
        public static int IndexOfFurthest(this List<Vector2> list, Vector2 point) => IndexOfFurthest(list, f => (f - point).sqrMagnitude);
        public static float MinimumDistance(this List<Vector2> list, Vector2 point) => MinimumDistance(list, f => (f - point).sqrMagnitude);
        public static float MaximumDistance(this List<Vector2> list, Vector2 point) => MaximumDistance(list, f => (f - point).sqrMagnitude);
        #endregion

        #region List of Vector2
        public static KeyValuePair<int, int> IndexOfClosestPair(this List<Vector3> list) => IndexOfClosestPair(list, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static KeyValuePair<int, int> IndexOfFurthestPair(this List<Vector3> list) => IndexOfFurthestPair(list, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static float SmallestDistance(this List<Vector3> list) => SmallestDistance(list, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static float LargestDistance(this List<Vector3> list) => LargestDistance(list, (f1, f2) => (f1 - f2).sqrMagnitude);
        public static int IndexOfClosest(this List<Vector3> list, Vector3 point) => IndexOfClosest(list, f => (f - point).sqrMagnitude);
        public static int IndexOfFurthest(this List<Vector3> list, Vector3 point) => IndexOfFurthest(list, f => (f - point).sqrMagnitude);
        public static float MinimumDistance(this List<Vector3> list, Vector3 point) => MinimumDistance(list, f => (f - point).sqrMagnitude);
        public static float MaximumDistance(this List<Vector3> list, Vector3 point) => MaximumDistance(list, f => (f - point).sqrMagnitude);
        #endregion

        #region List of Quaternion
        public static KeyValuePair<int, int> IndexOfClosestPair(this List<Quaternion> list) => IndexOfClosestPair(list, (f1, f2) => Quaternion.Angle(f1, f2));
        public static KeyValuePair<int, int> IndexOfFurthestPair(this List<Quaternion> list) => IndexOfFurthestPair(list, (f1, f2) => Quaternion.Angle(f1, f2));
        public static float SmallestDistance(this List<Quaternion> list) => SmallestDistance(list, (f1, f2) => Quaternion.Angle(f1, f2));
        public static float LargestDistance(this List<Quaternion> list) => LargestDistance(list, (f1, f2) => Quaternion.Angle(f1, f2));
        public static int IndexOfClosest(this List<Quaternion> list, Quaternion point) => IndexOfClosest(list, f => Quaternion.Angle(f, point));
        public static int IndexOfFurthest(this List<Quaternion> list, Quaternion point) => IndexOfFurthest(list, f => Quaternion.Angle(f, point));
        public static float MinimumDistance(this List<Quaternion> list, Quaternion point) => MinimumDistance(list, f => Quaternion.Angle(f, point));
        public static float MaximumDistance(this List<Quaternion> list, Quaternion point) => MaximumDistance(list, f => Quaternion.Angle(f, point));
        #endregion
    }
}