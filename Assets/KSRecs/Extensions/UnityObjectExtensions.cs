using System;
using KSRecs.Utils;
using UnityEngine;

namespace KSRecs.UnityObjectExtensions
{
    public static class UnityExtensions
    {
        public static bool CanDestroy(this GameObject targetGo, Type componentToDestroy) => UnityObjectUtils.CanDestroy(targetGo, componentToDestroy);
        public static string ClassName(this Component component) => UnityObjectUtils.ClassName(component);
        public static string CopyTo(this Component sourceComp, GameObject targetObject, bool copyNonSerializedFieldsToo) => UnityObjectUtils.CopyComponentTo(sourceComp, targetObject, copyNonSerializedFieldsToo);
    }
}