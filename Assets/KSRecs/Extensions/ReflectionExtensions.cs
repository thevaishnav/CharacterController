using System;
using System.Collections.Generic;
using System.Reflection;
using KSRecs.Utils;

namespace KSRecs.Extensions
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> SelfAndBaseTypes(this object target) => ReflectionUtils.SelfAndBaseTypes(target);
        public static IEnumerable<MethodInfo> AllMethods(this object target, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) => ReflectionUtils.GetAllMethods(target, bindingFlags);
        public static IEnumerable<MethodInfo> AllMethodsByName(this object target, string name, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) => ReflectionUtils.GetAllMethodsByName(target, name, bindingFlags); 
        public static bool HasMethodByName(this object target, string name, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) => ReflectionUtils.HasMethodByName(target, name, bindingFlags);
        public static int CountMethodByName(this object target, string name, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) => ReflectionUtils.CountMethodOverloads(target, name, bindingFlags);
        public static IEnumerable<FieldInfo> AllFields(this object target, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) => ReflectionUtils.GetAllFields(target, bindingFlags);
        public static bool HasFieldByName(this object target, string name, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) => ReflectionUtils.HasFieldByName(target, name, bindingFlags);
        public static IEnumerable<PropertyInfo> AllProperties(this object target, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) => ReflectionUtils.GetAllProperties(target, bindingFlags);
        public static bool HasPropertyByName(this object target, string name, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) => ReflectionUtils.HasPropertyByName(target, name, bindingFlags);
        public static IEnumerable<MethodInfo> AllMethodsWithAttributes<TA>(this object target, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) where TA : Attribute => ReflectionUtils.GetAllMethodsWithAttributes<TA>(target, bindingFlags);
        public static IEnumerable<FieldInfo> AllFieldsWithAttributes<TA>(this object target, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) where TA : Attribute => ReflectionUtils.GetAllFieldsWithAttributes<TA>(target, bindingFlags);
        public static IEnumerable<PropertyInfo> AllPropertiesWithAttributes<TA>(this object target, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly) where TA : Attribute => ReflectionUtils.GetAllPropertiesWithAttributes<TA>(target, bindingFlags);
        public static object GetFieldValue(object obj, string fieldName, BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) => ReflectionUtils.GetFieldValue(obj, fieldName, bindings);
        public static bool SetFieldValue(object obj, string fieldName, object value, bool includeAllBases = false, BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) => ReflectionUtils.SetFieldValue(obj, fieldName, value, includeAllBases, bindings);
    }
}