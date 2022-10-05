using System;
using System.Reflection;

public static class ReflectionUtilities
{
    public static bool HasMethod(this object objectToCheck, string methodName)
    {
        Type type = objectToCheck.GetType();
        return type.GetMethod(methodName) != null;
    }

    public static MethodInfo[] GetAllMethods(this object target)
    {
        return target.GetType().GetMethods(BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    }

    public static bool QuickInvoke(this object target, string methodName)
    {
        try
        {
            target.GetType().GetMethod(methodName, BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Invoke(target, null);
            return true;
        }
        catch (NullReferenceException n)
        {
            return false;
        }
        catch (AmbiguousMatchException)
        {
            return TryInvokeMethod(target, methodName);
        }
    }
    
    public static bool TryInvokeMethod(this object target, string methodName)
    {
        MethodInfo[] methods = target.GetType().GetMethods(BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (MethodInfo methodInfo in methods)
        {
            if (methodInfo.Name == methodName && methodInfo.GetParameters().Length == 0)
            {
                methodInfo.Invoke(target, null);
                return true;
            }
        }
        return false;
    }
}