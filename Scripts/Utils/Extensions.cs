using System;
using System.Collections.Generic;
using System.Reflection;


namespace KS.CharaCon.Utils
{
    public static class Extensions
    {
        /// <summary> Loop through all classes that derive form certain class </summary>
        /// <param name="type"> Parent class type </param>
        public static IEnumerable<Type> AllChildClasses(this Type type)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type classType in assembly.GetTypes())
                {
                    if (classType.IsSubclassOf(type) && !classType.IsAbstract)
                    {
                        yield return classType;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the child class override a method defined in the parent class
        /// </summary>
        /// <param name="childClassInstance"> Any instance of child class </param>
        /// <param name="methodName"> Name of method to check </param>
        /// <typeparam name="TParent"> Type of parent class </typeparam>
        /// <returns> true if child class override the method </returns>
        public static bool DoesChildOverride<TParent>(this TParent childClassInstance, string methodName)
        {
            MethodInfo method = childClassInstance.GetType().GetMethod(methodName, BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (method == null)
                return false;
            return method.DeclaringType != typeof(TParent);
        }

        /// <summary>
        /// Checks if the child class override a method defined in the parent class
        /// </summary>
        /// <param name="methodName"> Name of method to check </param>
        /// <typeparam name="TParent"> Type of parent class </typeparam>
        /// <typeparam name="TChild"> Type of child class </typeparam>
        /// <returns> true if child class override the method </returns>
        public static bool DoesChildOverride<TParent, TChild>(string methodName)
        {
            MethodInfo method = typeof(TChild).GetMethod(methodName, BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (method == null)
                return false;
            return method.DeclaringType != typeof(TParent);
        }
        
        public static IEnumerable<TOther> ChildOverrides<TParent, TOther>(this TParent childClassInstance, Dictionary<string, TOther> methodsToCheck)
        {
            MethodInfo[] allMethods = childClassInstance.GetType().GetMethods(BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (MethodInfo method in allMethods)
            {
                string methodName = method.Name;
                if (methodsToCheck.ContainsKey(methodName) && method.DeclaringType != typeof(TParent))
                {
                    yield return methodsToCheck[methodName];
                }
            }
        }
    }
}