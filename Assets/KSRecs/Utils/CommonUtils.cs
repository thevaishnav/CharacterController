using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSRecs.Utils
{
    public static class CommonUtils
    {
        public static T CastObjectTo<T>(object value)
        {
            return (T) Convert.ChangeType(value, typeof(T));
        }

        public static void StartCoroutine(IEnumerator routine)
        {
            new GameObject(nameof(routine) + " Coroutine").AddComponent<CommonUtilsObject>().StartCustomCoroutine(routine);
        }
    }
}