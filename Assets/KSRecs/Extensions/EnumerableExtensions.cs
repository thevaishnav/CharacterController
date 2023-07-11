using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace KSRecs.Extensions
{
    public static class EnumerableExtensions
    {
        // @formatter:off
        public static T       RandomElement<T>(this List<T> list)                                     => list[Random.Range(0, list.Count)];
        public static void    RandomizeInPlace<T>(this List<T> list)                                  => EUI.RandomizeInPlace(new ListWrapper<T>(list));
        public static List<T> Randomized<T>(this List<T> list)                                        => ((ListWrapper<T>) EUI.Randomized(new ListWrapper<T>(list))).List;
        public static int     ToLoopingIndex<T>(this List<T> list, int index)                         => EUI.ToLoopingIndex(index, list.Count);
        public static T       ElementAtLoopingIndex<T>(this List<T> list, int index)                  => list[EUI.ToLoopingIndex(index, list.Count)];
        public static List<T> PySubList<T>(this List<T> list, int start, int end = -1, int step = 1)  => ((ListWrapper<T>) EUI.PySubArray(new ListWrapper<T>(list), start, end, step)).List;
        
        public static T    RandomElement<T>(this T[] list)                                    => list[Random.Range(0, list.Length)];
        public static void RandomizeInPlace<T>(this T[] list)                                 => EUI.RandomizeInPlace(new ArrayWrapper<T>(list));
        public static T[]  Randomized<T>(this T[] list)                                       => ((ArrayWrapper<T>) EUI.Randomized(new ArrayWrapper<T>(list))).Array;
        public static int  ToLoopingIndex<T>(this T[] list, int index)                        => EUI.ToLoopingIndex(index, list.Length);
        public static T    ElementAtLoopingIndex<T>(this T[] list, int index)                 => list[EUI.ToLoopingIndex(index, list.Length)];
        public static T[]  PySubList<T>(this T[] list, int start, int end = -1, int step = 1) => ((ArrayWrapper<T>) EUI.PySubArray(new ArrayWrapper<T>(list), start, end, step)).Array;
        
        public static Pair<float>        ClosestPair(this float[] array)             => EUI.ClosestPair(new ArrayWrapper<float>(array), (f1, f2) => Mathf.Abs(f1 - f2));
        public static Pair<float>        FurthestPair(this float[] array)            => EUI.FurthestPair(new ArrayWrapper<float>(array), (f1, f2) => Mathf.Abs(f1 - f2));
        public static ElementInfo<float> ClosestTo(this float[] array, float point)  => EUI.ClosestTo(array, f => Mathf.Abs(point - f));
        public static ElementInfo<float> FurthestTo(this float[] array, float point) => EUI.FurthestTo(array, f => Mathf.Abs(point - f));
        
        public static Pair<int>        ClosestPair(this int[] array)           => EUI.ClosestPair(new ArrayWrapper<int>(array), (f1, f2) => Mathf.Abs(f1 - f2));
        public static Pair<int>        FurthestPair(this int[] array)          => EUI.FurthestPair(new ArrayWrapper<int>(array), (f1, f2) => Mathf.Abs(f1 - f2));
        public static ElementInfo<int> ClosestTo(this int[] array, int point)  => EUI.ClosestTo(array, f => Mathf.Abs(point - f));
        public static ElementInfo<int> FurthestTo(this int[] array, int point) => EUI.FurthestTo(array, f => Mathf.Abs(point - f));
        
        public static Pair<Vector2>        ClosestPair(this Vector2[] array)               => EUI.ClosestPair(new ArrayWrapper<Vector2>(array), (f1, f2) => (f1 - f2).sqrMagnitude);
        public static Pair<Vector2>        FurthestPair(this Vector2[] array)              => EUI.FurthestPair(new ArrayWrapper<Vector2>(array), (f1, f2) => (f1 - f2).sqrMagnitude);
        public static ElementInfo<Vector2> ClosestTo(this Vector2[] array, Vector2 point)  => EUI.ClosestTo(array, f => (f - point).sqrMagnitude);
        public static ElementInfo<Vector2> FurthestTo(this Vector2[] array, Vector2 point) => EUI.FurthestTo(array, f => (f - point).sqrMagnitude);
        
        public static Pair<Vector3>        ClosestPair(this Vector3[] array)               => EUI.ClosestPair(new ArrayWrapper<Vector3>(array), (f1, f2) => (f1 - f2).sqrMagnitude);
        public static Pair<Vector3>        FurthestPair(this Vector3[] array)              => EUI.FurthestPair(new ArrayWrapper<Vector3>(array), (f1, f2) => (f1 - f2).sqrMagnitude);
        public static ElementInfo<Vector3> ClosestTo(this Vector3[] array, Vector3 point)  => EUI.ClosestTo(array, f => (f - point).sqrMagnitude);
        public static ElementInfo<Vector3> FurthestTo(this Vector3[] array, Vector3 point) => EUI.FurthestTo(array, f => (f - point).sqrMagnitude);
        
        public static Pair<Quaternion>        ClosestPair(this Quaternion[] array)                  => EUI.ClosestPair(new ArrayWrapper<Quaternion>(array), (f1, f2) => Quaternion.Angle(f1, f2));
        public static Pair<Quaternion>        FurthestPair(this Quaternion[] array)                 => EUI.FurthestPair(new ArrayWrapper<Quaternion>(array), (f1, f2) => Quaternion.Angle(f1, f2));
        public static ElementInfo<Quaternion> ClosestTo(this Quaternion[] array, Quaternion point)  => EUI.ClosestTo(array, f => Quaternion.Angle(f, point));
        public static ElementInfo<Quaternion> FurthestTo(this Quaternion[] array, Quaternion point) => EUI.FurthestTo(array, f => Quaternion.Angle(f, point));
        
        public static Pair<float>        ClosestPair(this List<float> array)             => EUI.ClosestPair(new ListWrapper<float>(array), (f1, f2) => Mathf.Abs(f1 - f2));
        public static Pair<float>        FurthestPair(this List<float> array)            => EUI.FurthestPair(new ListWrapper<float>(array), (f1, f2) => Mathf.Abs(f1 - f2));
        public static ElementInfo<float> ClosestTo(this List<float> array, float point)  => EUI.ClosestTo(array, f => Mathf.Abs(point - f));
        public static ElementInfo<float> FurthestTo(this List<float> array, float point) => EUI.FurthestTo(array, f => Mathf.Abs(point - f));
        
        public static Pair<int>        ClosestPair(this List<int> array)           => EUI.ClosestPair(new ListWrapper<int>(array), (f1, f2) => Mathf.Abs(f1 - f2));
        public static Pair<int>        FurthestPair(this List<int> array)          => EUI.FurthestPair(new ListWrapper<int>(array), (f1, f2) => Mathf.Abs(f1 - f2));
        public static ElementInfo<int> ClosestTo(this List<int> array, int point)  => EUI.ClosestTo(array, f => Mathf.Abs(point - f));
        public static ElementInfo<int> FurthestTo(this List<int> array, int point) => EUI.FurthestTo(array, f => Mathf.Abs(point - f));
        
        public static Pair<Vector2>        ClosestPair(this List<Vector2> array)               => EUI.ClosestPair(new ListWrapper<Vector2>(array), (f1, f2) => (f1 - f2).sqrMagnitude);
        public static Pair<Vector2>        FurthestPair(this List<Vector2> array)              => EUI.FurthestPair(new ListWrapper<Vector2>(array), (f1, f2) => (f1 - f2).sqrMagnitude);
        public static ElementInfo<Vector2> ClosestTo(this List<Vector2> array, Vector2 point)  => EUI.ClosestTo(array, f => (f - point).sqrMagnitude);
        public static ElementInfo<Vector2> FurthestTo(this List<Vector2> array, Vector2 point) => EUI.FurthestTo(array, f => (f - point).sqrMagnitude);
        
        public static Pair<Vector3>        ClosestPair(this List<Vector3> array)               => EUI.ClosestPair(new ListWrapper<Vector3>(array), (f1, f2) => (f1 - f2).sqrMagnitude);
        public static Pair<Vector3>        FurthestPair(this List<Vector3> array)              => EUI.FurthestPair(new ListWrapper<Vector3>(array), (f1, f2) => (f1 - f2).sqrMagnitude);
        public static ElementInfo<Vector3> ClosestTo(this List<Vector3> array, Vector3 point)  => EUI.ClosestTo(array, f => (f - point).sqrMagnitude);
        public static ElementInfo<Vector3> FurthestTo(this List<Vector3> array, Vector3 point) => EUI.FurthestTo(array, f => (f - point).sqrMagnitude);
        
        public static Pair<Quaternion>        ClosestPair(this List<Quaternion> array)                  => EUI.ClosestPair(new ListWrapper<Quaternion>(array), (f1, f2) => Quaternion.Angle(f1, f2));
        public static Pair<Quaternion>        FurthestPair(this List<Quaternion> array)                 => EUI.FurthestPair(new ListWrapper<Quaternion>(array), (f1, f2) => Quaternion.Angle(f1, f2));
        public static ElementInfo<Quaternion> ClosestTo(this List<Quaternion> array, Quaternion point)  => EUI.ClosestTo(array, f => Quaternion.Angle(f, point));
        public static ElementInfo<Quaternion> FurthestTo(this List<Quaternion> array, Quaternion point) => EUI.FurthestTo(array, f => Quaternion.Angle(f, point));
        // @formatter:on
    }
}